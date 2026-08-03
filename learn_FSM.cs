//? What is a Finite State Machine (FSM)?
// A model of something that can only ever be in exactly one of a fixed, known set of "states" at a time,
// and moves between states only via explicitly allowed "transitions." Anything not explicitly allowed is
// forbidden by default. That last part is the whole point — an FSM is a way of making illegal states and
// illegal moves impossible (or at least loud) instead of relying on scattered if-checks to catch them.

// Real-world FSMs you already know:
// - A traffic light: Red -> Green -> Yellow -> Red. You can never jump Red -> Yellow directly.
// - An order: Placed -> Shipped -> Delivered, with Cancelled reachable from Placed/Shipped but not Delivered.
// - A pull request: Draft -> Open -> Merged/Closed. You can't "un-merge" back to Open.

// This project models User account lifecycle as an FSM:
//   Models/UserStatus.cs          -> the states (enum)
//   StateMachine/UserStateMachine.cs -> the transitions (which moves are legal)
//   Models/User.cs                -> the Status property the FSM controls
//   Services/Implementations/UserService.cs -> ChangeUserStatusAsync (load -> transition -> save)
//   Controllers/UserController.cs -> PATCH /api/user/{id}/status (the HTTP entry point)


/*_______________________________________________________________________________________________________________________________ */

//? Why not just use a bool like the existing IsActive field?

// A bool only answers "is this on or off" — it can't represent "why is it off" or "which off-states can
// this recover from." Compare:

// IsActive = false
// -> Was the user suspended? Deleted? Never verified their email? You can't tell, and worse, nothing stops
//    you from flipping IsActive back to true on an account that should stay closed forever.

// Status = UserStatus.Deactivated
// -> Unambiguous, and the FSM's transition table can say "Deactivated has no way out" (see below), so a bug
//    that tries to reactivate a deactivated account fails loudly with an exception instead of silently
//    "working" and undoing an intentional account closure.

// Rule of thumb: reach for an FSM the moment a boolean or a loose string field starts growing informal rules
// like "well, IsActive can only go back to true if it was suspended, not if it was deleted." That sentence
// IS a transition table — it deserves to be written as one, not buried in scattered if-statements.


/*_______________________________________________________________________________________________________________________________ */

//? The states (Models/UserStatus.cs)

// public enum UserStatus
// {
//     PendingVerification,   // just signed up, hasn't confirmed email yet
//     Active,                // normal, fully-functional account
//     Suspended,             // temporarily disabled (e.g. abuse report under review) — reversible
//     Deactivated            // permanently closed — terminal, no way out
// }

// Diagram of the legal moves (arrows = allowed transitions):
//
//   PendingVerification --> Active --> Suspended
//          |                  |            |
//          v                  v            v
//     Deactivated  <----------+------------+
//
// Active <-> Suspended is a two-way arrow (reversible).
// Everything eventually can reach Deactivated, but nothing leaves it.


/*_______________________________________________________________________________________________________________________________ */

//? The transition table (StateMachine/UserStateMachine.cs)

// private static readonly Dictionary<UserStatus, UserStatus[]> _allowedTransitions = new()
// {
//     [UserStatus.PendingVerification] = new[] { UserStatus.Active, UserStatus.Deactivated },
//     [UserStatus.Active]              = new[] { UserStatus.Suspended, UserStatus.Deactivated },
//     [UserStatus.Suspended]           = new[] { UserStatus.Active, UserStatus.Deactivated },
//     [UserStatus.Deactivated]         = Array.Empty<UserStatus>(),   // terminal — explicitly empty
// };

//# Why a Dictionary<TState, TState[]> instead of a big switch/if-chain?
// A table is data, not logic — you can read the entire ruleset in one glance, diff it in a code review,
// and unit test it by looping over every (from, to) pair. A switch statement with the same rules buried
// inside it hides the "shape" of the state machine behind imperative code.

//# Why does Deactivated map to an empty array instead of being left out of the dictionary entirely?
// Leaving it out and having CanTransition() return false via TryGetValue failing works too (that's exactly
// what happens), but writing it explicitly documents intent: "yes, we thought about this state, and no,
// there is genuinely nothing it can transition to." A missing entry could just as easily be a bug (forgot
// to add a new state to the table) — this makes "no transitions" a deliberate choice, not an omission.

//# CanTransition vs Transition — why two methods?
// public static bool CanTransition(UserStatus from, UserStatus to)
// public static void Transition(User user, UserStatus to)
//
// CanTransition is a pure question ("would this be legal?") with no side effects — useful for, say,
// deciding whether to show a "Suspend" button in a UI at all. Transition is the actual mutation, and it
// throws (rather than silently no-op-ing or returning false) if the move is illegal, because a caller that
// requests an illegal transition almost always has a bug, and a thrown exception surfaces that immediately
// instead of quietly doing nothing.


/*_______________________________________________________________________________________________________________________________ */

//? Wiring it into the app (the flow of one request)

// PATCH /api/user/1/status   { "status": 1 }   // 1 = Active
//   |
//   v
// UserController.ChangeUserStatus(id, request)
//   |
//   v
// UserService.ChangeUserStatusAsync(id, newStatus)
//   1. Load the User from the DbContext
//   2. UserStateMachine.Transition(user, newStatus)  <-- all the business rule lives HERE, not in the
//                                                          controller or service
//   3. SaveChangesAsync()
//   |
//   v
// Controller catches InvalidOperationException -> 400 Bad Request with a clear message
// (anything else -> 200 OK with the updated user)

//# Why does the service only load/save, and push the actual decision into UserStateMachine?
// Single Responsibility: the service's job is orchestration (get the entity, ask the FSM, persist).
// The FSM's job is "is this move legal." If tomorrow you need the same rules enforced from a background
// job or a console admin tool (not just this one controller), you call UserStateMachine directly — the
// rules aren't trapped inside a web-request-shaped method.

//# Why is the request body { "status": 1 } instead of separate endpoints like /activate, /suspend?
// Both are legitimate designs:
// - One generic endpoint (what we built): the caller states the desired end state, the FSM decides if the
//   move from wherever the resource currently is counts as legal. Fewer endpoints, but the caller needs to
//   know what state they're requesting.
// - Verb-per-transition endpoints (/activate, /suspend, /reinstate): more RESTful/discoverable, one HTTP
//   route per business action, but you write (and maintain) one controller action per transition.
// Either way, the underlying FSM (transition table + guard) is identical — this is a controller-shape
// choice, not an FSM design choice.


/*_______________________________________________________________________________________________________________________________ */

//? Numeric vs string enum over the wire

// By default, System.Text.Json serializes C# enums as their underlying int, so the request/response looks
// like { "status": 1 } rather than { "status": "Active" }. That's what we tested against (PendingVerification=0,
// Active=1, Suspended=2, Deactivated=3 — the order they're declared in the enum).

// To get readable strings instead, register a converter in Program.cs:
// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//         options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
// After that, both request and response bodies would use "Active" instead of 1. Trade-off: strings are
// self-documenting in API responses/Swagger, but a like-for-like rename of an enum member becomes a
// breaking API change (whereas with numeric serialization, only reordering/removing members breaks callers).


/*_______________________________________________________________________________________________________________________________ */

//? A bug this exercise surfaced (unrelated to FSM, but blocked testing it)

// The Users table has a required RoleId foreign key added in an earlier migration, defaulted to 0 for
// existing rows — but no Role row with Id = 0 ever existed. That's a silent landmine: the FK constraint
// only fires the moment you try to INSERT/UPDATE a row referencing the missing key, so it looked fine right
// up until we tried to apply a new migration against a database with real data in it.
// Fixed by seeding a placeholder Role (Id = 0, "Unassigned") inside that same migration's Up() method —
// see Migrations/20260803040719_SyncModelChanges.cs. Lesson: a "required" FK with a hardcoded default value
// needs a matching seeded row, or every future insert that doesn't explicitly set that FK will eventually fail.


/*_______________________________________________________________________________________________________________________________ */

//? Verified transitions (ran these live against http://localhost:5250 during this exercise)

// PATCH /api/user/1/status {"status":1}  Pending -> Active        => 200 OK
// PATCH /api/user/1/status {"status":2}  Active -> Suspended       => 200 OK
// PATCH /api/user/1/status {"status":0}  Suspended -> Pending      => 400 "Cannot transition user 1 from 'Suspended' to 'PendingVerification'."
// PATCH /api/user/1/status {"status":1}  Suspended -> Active       => 200 OK
// PATCH /api/user/1/status {"status":3}  Active -> Deactivated     => 200 OK
// PATCH /api/user/1/status {"status":1}  Deactivated -> Active     => 400 "Cannot transition user 1 from 'Deactivated' to 'Active'."

// Notice the illegal attempts return 400 with a message naming the exact states involved — that's the
// InvalidOperationException thrown by UserStateMachine.Transition, caught in the controller. Nothing about
// this required "remembering" which transitions are illegal in the controller or service code.


/*_______________________________________________________________________________________________________________________________ */

//? When to reach for a real FSM library instead of hand-rolling a Dictionary

// The hand-rolled table-driven approach above is genuinely enough for most CRUD-app entity lifecycles
// (orders, users, tickets, subscriptions). Reach for a library like Stateless
// (https://github.com/dotnet-state-machine/stateless — a well-known, widely used .NET FSM library) once you
// need things a plain dictionary gets awkward at:
// - Entry/exit actions per state (e.g. "send an email whenever anyone enters Suspended")
// - Guard conditions on a transition (e.g. "Active -> Deactivated is only legal if the user has no unpaid invoices")
// - Hierarchical/nested states, or generating a visual diagram of the machine for documentation
// The core concept — states + a table of legal transitions + a guard that rejects anything not in the table
// — is identical either way. Stateless just gives you a fluent API and extra features on top of the same idea.

//TODO: Try adding a guard condition — e.g. block Active -> Suspended if the user was only just created in
// the last 5 minutes (a "grace period" rule) — directly inside UserStateMachine.Transition, and see how
// quickly a plain if-check bolted onto a table-driven FSM starts to feel like it wants Stateless's guard
// clause API instead.
