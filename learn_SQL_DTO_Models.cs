//? Models, ModelBuilder, DTOs, Migrations, and Scaffolding
// This file ties together four things that are easy to blur into one blob: the Model (what a row IS),
// the ModelBuilder (how EF is told to map that row to a table), the DTO (what crosses the wire), and
// Migrations/Scaffolding (the tooling that keeps code and database schema in sync).


/*================================================================================================================*/
//? PART 1 — Models & ModelBuilder
/*================================================================================================================*/

//? What is a "Model" in this codebase?
// A Model (Models/User.cs, Models/Product.cs, Models/Role.cs, Models/Address.cs) is a plain C# class
// (a POCO — Plain Old CLR Object) that EF Core maps 1:1 to a database table via a DbSet<T> on
// AppDbContext (see Data/AppDbContext.cs). Every property becomes a column; every instance is a row.
// This is also called the "entity" or "domain model" — the single source of truth for the shape of your
// data at rest.

// public class User
// {
//     [Key] public int Id { get; set; }
//     [Required][MaxLength(50)] public string FirstName { get; set; }
//     public int RoleId { get; set; }
//     public Role Role { get; set; } = null!;
//     public UserStatus Status { get; set; } = UserStatus.PendingVerification;
// }

//# Two ways to configure how a Model maps to the database
// 1. Data Annotations — attributes directly on the property ([Required], [MaxLength(50)], [Key],
//    [ForeignKey], [Column("first_name")], [NotMapped]...). Quick, colocated with the property, easy to
//    scan — this is what most of Models/User.cs uses today.
// 2. Fluent API (ModelBuilder) — configuration written in AppDbContext.OnModelCreating instead of on the
//    class. More powerful and composable, and it's the ONLY option for a few things annotations can't
//    express at all (see below).

//? Why do we need ModelBuilder if annotations already cover Required/MaxLength/Key/ForeignKey?
// Because some mapping rules have no attribute equivalent. Concretely, in AppDbContext.OnModelCreating:

// protected override void OnModelCreating(ModelBuilder modelBuilder)
// {
//     base.OnModelCreating(modelBuilder);
//
//     // 1. Composite (multi-column) primary key — no [Key] combination does this
//     modelBuilder.Entity<OrderLine>()
//         .HasKey(ol => new { ol.OrderId, ol.ProductId });
//
//     // 2. A relationship with an explicit FK, cascade behavior, and required-ness spelled out
//     modelBuilder.Entity<User>()
//         .HasOne(u => u.Role)
//         .WithMany(r => r.Users)
//         .HasForeignKey(u => u.RoleId)
//         .OnDelete(DeleteBehavior.Restrict);   // block deleting a Role that still has Users
//
//     // 3. A unique constraint across ONE OR MORE columns — [Index] alone can't express "unique"
//     //    cleanly pre-EF-Core-5, and multi-column uniqueness never had an annotation
//     modelBuilder.Entity<User>()
//         .HasIndex(u => u.Email)
//         .IsUnique();
//
//     // 4. Seed data — rows that must exist the moment the table is created (see the Role Id=0
//     //    "Unassigned" seed added in Migrations/20260803040719_SyncModelChanges.cs, and learn_FSM.cs's
//     //    note on why a required FK with a hardcoded default needs a matching seeded row)
//     modelBuilder.Entity<Role>().HasData(
//         new Role { Id = 0, Name = "Unassigned" }
//     );
//
//     // 5. A global query filter — e.g. silently exclude soft-deleted rows from every query without
//     //    remembering to add .Where(u => !u.IsDeleted) at every call site
//     modelBuilder.Entity<User>()
//         .HasQueryFilter(u => !u.IsDeleted);
//
//     // 6. Precision/scale for decimals, table name overrides, owned types, table splitting, etc.
//     modelBuilder.Entity<Product>()
//         .Property(p => p.Price)
//         .HasPrecision(10, 2);
// }

//# Rule of thumb: annotations for per-property, single-entity rules; ModelBuilder for anything that
// spans multiple properties/entities (composite keys, relationships, indexes across columns), anything
// with no attribute (seed data, query filters, cascade behavior), or anything you'd rather keep OUT of
// the entity class entirely (see IEntityTypeConfiguration<T> below).

//# Keeping ModelBuilder from becoming one giant OnModelCreating method
// Once you have more than a couple of entities, put each entity's Fluent API config in its own class
// implementing IEntityTypeConfiguration<T>, then load them all with one line:
//
//     public class UserConfiguration : IEntityTypeConfiguration<User>
//     {
//         public void Configure(EntityTypeBuilder<User> builder)
//         {
//             builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
//             builder.HasIndex(u => u.Email).IsUnique();
//         }
//     }
//
//     // in OnModelCreating:
//     modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
//
// This is the ModelBuilder equivalent of "one controller per resource" — each entity's mapping rules
// live in their own file instead of all competing for space in one method.


/*----------------------------------------------------------------------------------------------------------*/

//? Model-specific methods vs. generic methods that work across all models

//# Model-specific methods (belong ON the entity itself)
// A method belongs on the model class when it's pure domain logic that only needs that entity's own
// properties — no DbContext, no I/O, just computing something from data the object already has:
//
//     public class Product
//     {
//         public decimal Price { get; set; }
//
//         public decimal CalculateTotalPrice(decimal discountPercentage)
//         {
//             if (discountPercentage is < 0 or > 100)
//                 throw new ArgumentOutOfRangeException(nameof(discountPercentage));
//             return Price - Price * (discountPercentage / 100);
//         }
//     }
//
// This project's FSM is the same idea taken further: UserStateMachine.Transition(user, newStatus) is
// domain logic specific to User's Status field (see learn_FSM.cs) — it just lives in its own static
// class instead of on User directly, because the rule involves a lookup table, not just User's own
// fields.

//# Generic methods that need to work across EVERY model (not specific to one)
// You can't put "shared" logic on the base model classes unless they share a base type or interface —
// C# generics need something to constrain <T> to. Two common patterns:
//
// 1. A marker interface implemented by every entity, then a generic constrained method:
//
//     public interface IEntity { int Id { get; set; } }
//     public class User : IEntity { public int Id { get; set; } /* ... */ }
//     public class Product : IEntity { public int Id { get; set; } /* ... */ }
//
//     public static class EntityExtensions
//     {
//         public static bool IsNew<TEntity>(this TEntity entity) where TEntity : IEntity
//             => entity.Id == 0;
//     }
//
//     // Works for ANY entity that implements IEntity:
//     if (user.IsNew()) { ... }
//     if (product.IsNew()) { ... }
//
// 2. A generic repository/service base class that every model-specific repository inherits from —
//    this is how you get GetById/Add/Update/Delete "for free" without rewriting them per entity:
//
//     public class GenericRepository<TEntity> where TEntity : class, IEntity
//     {
//         protected readonly AppDbContext _context;
//         public GenericRepository(AppDbContext context) => _context = context;
//
//         public async Task<TEntity?> GetByIdAsync(int id) => await _context.Set<TEntity>().FindAsync(id);
//         public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);
//     }
//
//     public class UserRepository : GenericRepository<User>
//     {
//         public UserRepository(AppDbContext context) : base(context) { }
//         // Add ONLY User-specific queries here, e.g.:
//         public Task<User?> GetByEmailAsync(string email) =>
//             _context.Users.FirstOrDefaultAsync(u => u.Email == email);
//     }
//
//# Why bother splitting it this way instead of writing GetById/Add on every repository?
// Same reason as IEntityTypeConfiguration<T> above: the generic base class is where "true for every
// entity" logic lives (CRUD basics), and the specific subclass is where "only true for this entity"
// logic lives (GetByEmailAsync only makes sense for User). Mixing the two into one flat class per
// entity means copy-pasting the same GetById/Add/Update/Delete boilerplate N times.


/*================================================================================================================*/
//? PART 2 — DTOs (Request & Response)
/*================================================================================================================*/

//? What is a DTO, and why not just send the Model over the wire?
// A DTO (Data Transfer Object) is a class whose only job is to shape data crossing a boundary (HTTP
// request/response, in this app's case) — it carries no behavior, no EF tracking, no navigation
// properties full of lazy-loaded surprises. Sending the raw Model (User) directly to/from the client
// instead of a DTO causes real, specific problems:
//
// 1. Overposting: if a controller model-binds the request body directly into a User entity, a client
//    can set fields they should never control — e.g. POST { "id": 1, "isActive": true, "status": 3 }
//    could let a caller silently reactivate or promote an account, because User has those settable
//    properties and nothing stopped the bind. A RequestDTO only exposes the fields you explicitly
//    declared on it (see CreateUserRequest below), so there's nothing extra to overpost into.
// 2. Leaking internal/sensitive data: User might carry a PasswordHash, RoleId, or navigation properties
//    you never want serialized straight to a client. A ResponseDTO is an explicit allow-list.
// 3. Shape mismatch per endpoint: a list endpoint might want a slim { Id, FirstName, Status }, while a
//    single-record GET wants the full profile — one Model can't have two different JSON shapes, but two
//    different DTOs can.
// 4. Decoupling schema changes from API contract: renaming a DB column shouldn't have to break every
//    client overnight if the DTO property name stays the same, and vice versa.

//? "How are DTOs different from Request Models / Response Models?" — they're not; same concept, different name
// "RequestDTO" / "ResponseDTO" (this project's naming — see the RequestDTO/ and ResponseDTO/ folders) and
// "Request Model" / "Response Model" (a name you'll see in other codebases/tutorials) refer to the exact
// same pattern: a class shaped specifically for one direction of one endpoint. Other names for the same
// idea you'll run into: "ViewModel" (classic ASP.NET MVC term, when the class also drives Razor views),
// "Contract" (common in some API-first shops), "Command"/"Query" (when paired with MediatR + CQRS — see
// learn_Net_basics.cs's MediatR/CQRS note). None of these are a DIFFERENT technical mechanism from a
// DTO — they're naming conventions layered on the same "a plain class shaped for one side of a boundary"
// idea. Pick one convention per project and stay consistent (this project uses RequestDTO/ResponseDTO).

//# This project's actual example: Model -> RequestDTO -> Model -> ResponseDTO
//
// Entity  (Models/User.cs):
//   public class User
//   {
//       public int Id { get; set; }
//       public string FirstName { get; set; }
//       public string? LastName { get; set; }
//       public string Email { get; set; }
//       public string PhoneNumber { get; set; }
//       public int RoleId { get; set; }
//       public bool IsActive { get; set; } = true;
//       public UserStatus Status { get; set; } = UserStatus.PendingVerification;
//       public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
//       public DateTime? UpdatedDate { get; set; }
//   }
//
// RequestDTO — what the CLIENT is allowed to send in (RequestDTO/CreateUserRequest.cs):
//   public class CreateUserRequest
//   {
//       [Required][MaxLength(100)] public string FirstName { get; set; }
//       [MaxLength(100)] public string? LastName { get; set; }
//       [Required][MaxLength(150)][EmailAddress] public string Email { get; set; }
//       public string? PhoneNumber { get; set; }
//       // Notice: no Id, no IsActive, no Status, no RoleId, no CreatedDate — the client has no way to
//       // set any of those, by construction, not by convention.
//   }
//
// ResponseDTO — what the SERVER is allowed to send out (ResponseDTO/UserResponse.cs):
//   public class UserResponse
//   {
//       public int Id { get; set; }
//       public string FirstName { get; set; } = string.Empty;
//       public string? LastName { get; set; }
//       public DateTime CreatedDate { get; set; }
//       public bool IsActive { get; set; }
//       public UserStatus Status { get; set; }
//       // Notice: Email/PhoneNumber are commented out in the real file — an explicit choice about what
//       // this particular response shape exposes; RoleId/navigation properties aren't here either.
//   }

//# Where the Model <-> DTO mapping actually happens
// Helpers/MappingHelper.cs — static extension methods, one direction each:
//
//   public static User ToUser(this CreateUserRequest request) => new User { FirstName = request.FirstName, ... };
//   public static UserResponse ToUserResponse(this User user) => new UserResponse { Id = user.Id, ... };
//   public static void UpdateFromRequest(this User user, UpdateUserRequest request) { user.FirstName = request.FirstName; ...; user.UpdatedDate = DateTime.UtcNow; }
//
// Controller/Service flow for "create a user":
//   CreateUserRequest (from client JSON)
//        --request.ToUser()-->  User (new entity, unsaved)
//        --_context.Users.Add(user); SaveChangesAsync()-->  User (now has an Id, CreatedDate, etc.)
//        --user.ToUserResponse()-->  UserResponse (sent back to client as JSON)
//
//# Manual mapping (what this project does) vs. a mapping library (AutoMapper, Mapster)
// Manual extension methods (as above) are explicit and easy to debug/step through, but you write and
// maintain them by hand for every DTO pair. A library like AutoMapper lets you declare
// CreateMap<User, UserResponse>() once and calls _mapper.Map<UserResponse>(user) everywhere, which
// scales better once you have dozens of DTOs, at the cost of "magic" that's harder to breakpoint into
// and a runtime failure (missing mapping) instead of a compile-time one if you forget a property.


/*================================================================================================================*/
//? PART 3 — Migrations
/*================================================================================================================*/

//? What problem do migrations solve?
// Your Model classes describe the schema you WANT; migrations are the mechanism that gets an actual
// database from "whatever schema it currently has" to "the schema the Models now describe," without
// dropping and recreating the database (which would destroy existing data). Each migration is a
// timestamped, ordered, reversible step — Migrations/20260803040719_SyncModelChanges.cs,
// Migrations/20260803042336_AddUserStatus.cs, etc. in this project — that EF Core can apply forward or
// (in principle) roll back.

//# The core command pair
//   dotnet ef migrations add <Name>     // 1. Diff current Models against the last migration snapshot,
//                                        //    generate a new migration file with the difference.
//   dotnet ef database update           // 2. Actually run the pending migration(s) against the real DB.
//
// Step 1 does NOT touch the database — it only generates code. Step 2 is the one with real effect.
// This project's history shows the natural rhythm: InitialCreate -> AddUserModel -> UpdateModel ->
// SyncModelChanges -> AddUserStatus, one migration per meaningful model change.

//# What's actually inside a migration file
// Two generated methods, Up() and Down():
//
//     public partial class AddUserStatus : Migration
//     {
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.AddColumn<int>(name: "Status", table: "Users", nullable: false, defaultValue: 0);
//         }
//
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.DropColumn(name: "Status", table: "Users");
//         }
//     }
//
// Up() is what `dotnet ef database update` runs going forward; Down() is what runs if you roll back to
// an earlier migration (`dotnet ef database update <PreviousMigrationName>`) — EF generates both from
// the model diff, but you can hand-edit either (e.g. to add the Role Id=0 seed row inside a migration's
// Up(), as this project did in SyncModelChanges specifically to avoid a broken FK on existing rows —
// see learn_FSM.cs's note on that bug).

//# AppDbContextModelSnapshot.cs — what is this file and why shouldn't I hand-edit it?
// It's EF Core's running record of "what the model looked like after the last migration" — every new
// `migrations add` diffs your current Models against this snapshot, then updates it. It's generated,
// checked into source control, and should be treated like any other generated file: don't hand-edit it;
// let the tooling regenerate it. If it drifts out of sync with your actual Models, EF Core throws a
// clear warning at startup/next migration ("model changes pending") rather than silently misbehaving.

//# Undoing a migration you haven't applied yet vs. one you have
//   dotnet ef migrations remove          // Deletes the LAST migration file (only safe if you haven't
//                                         // run `database update` for it yet — otherwise the DB and
//                                         // your migration history table disagree).
//   dotnet ef database update <PriorMigrationName>   // Rolls the DATABASE back to an earlier migration
//                                                     // (runs Down() for everything after it). The
//                                                     // migration files themselves stay on disk;
//                                                     // `migrations remove` is a separate, file-level
//                                                     // operation.

//# Multiple model changes in one migration vs. one migration per change
// `dotnet ef migrations add` always captures EVERYTHING that changed in your Models since the last
// migration, in a single file — there's no way to "partially" migration one property at a time in one
// command. So the real choice is discipline: run `migrations add` after each logical change (one column,
// one relationship) for small, reviewable migrations, or let several changes pile up and run it once for
// a bigger migration. Smaller/more frequent migrations are easier to review and roll back individually;
// this project's history (AddUserModel, then UpdateModel, then SyncModelChanges, then AddUserStatus)
// favors the frequent/small approach.

//# Generating the raw SQL instead of letting EF run it
//   dotnet ef migrations script                    // Prints the SQL for ALL pending migrations
//   dotnet ef migrations script <From> <To>         // Prints SQL for a specific range
// Useful when a DBA needs to review/run the SQL by hand against a production database instead of letting
// the application apply `database update` directly at deploy time.


/*================================================================================================================*/
//? PART 4 — Scaffolding
/*================================================================================================================*/

//? "Scaffolding" is used for TWO different things in the EF/ASP.NET ecosystem — don't conflate them.

//# Scaffolding #1 — Database → Code ("reverse engineering" an existing database into Models)
//   dotnet ef dbcontext scaffold "<ConnectionString>" Microsoft.EntityFrameworkCore.SqlServer -o Models
//
// Use case: you're handed an EXISTING database (someone else's schema, a legacy system, a DB a DBA
// already owns) and want C# Model classes + a DbContext generated FROM it, instead of writing them by
// hand. EF Core reads the schema (tables, columns, FKs, indexes) and generates:
//   - One class per table (e.g. a Products table -> a generated Product.cs)
//   - A DbContext with a DbSet<T> per table and OnModelCreating Fluent API calls describing the schema
//     it found (this is often where you'll see ModelBuilder calls you never wrote by hand)
// This is the "database-first" workflow — the database is the source of truth, code follows it.

//# Scaffolding #2 — Model → Controller (generating CRUD controller/action boilerplate)
//   dotnet aspnet-codegenerator controller -name ProductController -m Product -dc AppDbContext ^
//       --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
//
// Requires: dotnet tool install -g dotnet-aspnet-codegenerator, plus the
// Microsoft.VisualStudio.Web.CodeGeneration.Design package in the project.
//
// Use case: you already have a Model (Product) and a DbContext, and want a controller with
// Get/GetById/Post/Put/Delete actions generated for you instead of typing the same CRUD skeleton by
// hand for every entity. --useDefaultLayout/--referenceScriptLibraries only matter for MVC (they scaffold
// Razor views too); for a pure Web API you'd typically drop those flags and only get the controller.
// This is the "code-first, then generate the boilerplate on top" workflow — the OPPOSITE direction of
// Scaffolding #1 (there, the database drove the code; here, the Model drives the controller).

//# So which "direction" does Migrations fit into?
// Migrations is Code -> Database (you already own the Model, and generate/apply the schema from it) —
// the mirror image of Scaffolding #1 (Database -> Code). Summary of all three directions:
//
//   Database -> Code         : dotnet ef dbcontext scaffold ...          ("DB-first" scaffolding)
//   Code -> Database         : dotnet ef migrations add / database update  (Migrations)
//   Model -> Controller code : dotnet aspnet-codegenerator controller ...  ("MVC/API" scaffolding)

//# When is scaffolding actually worth it vs. hand-writing (as this project does)?
// Scaffolded controllers assume a close-to-1:1 mapping between Model and controller action, don't know
// about your RequestDTO/ResponseDTO split, and don't wire up things like [Authorize], the FSM guard in
// UserStateMachine, or custom validation — you'd hand-edit the generated file anyway for anything beyond
// the most generic CRUD. It's most valuable for: fast prototyping, internal admin tools where 1:1
// Model-to-endpoint is actually fine, or onboarding onto a large legacy database (Scaffolding #1) where
// hand-typing hundreds of columns would be pure toil. For a real product API with DTOs, an FSM, and
// custom business rules — like this one — hand-writing the controller/service layer (as UserController /
// UserService already are) gives you more control for a small amount of extra typing.
