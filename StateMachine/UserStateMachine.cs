using ProductApp.Models;

namespace ProductApp.StateMachine
{
  // A Finite State Machine (FSM) is just two things:
  //   1. A fixed set of states       -> UserStatus enum
  //   2. A table of legal transitions -> _allowedTransitions below
  //
  // Instead of scattering "if user.IsActive and not banned, allow suspend" checks across
  // services/controllers, every legal move is declared once, here, in one place. Anything
  // not listed is illegal by default — the FSM fails closed, not open.
  public static class UserStateMachine
  {
    private static readonly Dictionary<UserStatus, UserStatus[]> _allowedTransitions = new()
    {
      // PendingVerification -> Active: user confirms their email
      // PendingVerification -> Deactivated: admin removes an unverified signup
      [UserStatus.PendingVerification] = new[] { UserStatus.Active, UserStatus.Deactivated },

      // Active -> Suspended: temporary, reversible (e.g. abuse report under review)
      // Active -> Deactivated: permanent, e.g. user closes their own account
      [UserStatus.Active] = new[] { UserStatus.Suspended, UserStatus.Deactivated },

      // Suspended -> Active: reinstated after review
      // Suspended -> Deactivated: review concluded the account should be closed
      [UserStatus.Suspended] = new[] { UserStatus.Active, UserStatus.Deactivated },

      // Deactivated is terminal — no way out. Model this by giving it an empty array
      // rather than omitting it, so the "no transitions allowed" behavior is explicit.
      [UserStatus.Deactivated] = Array.Empty<UserStatus>(),
    };

    public static bool CanTransition(UserStatus from, UserStatus to)
    {
      return _allowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
    }

    // Applies the transition in-memory (caller still owns calling SaveChangesAsync).
    // Throwing on an illegal transition — rather than silently ignoring it or returning false —
    // means a bug that attempts one can never be mistaken for a no-op; it surfaces immediately.
    public static void Transition(User user, UserStatus to)
    {
      if (!CanTransition(user.Status, to))
      {
        throw new InvalidOperationException(
            $"Cannot transition user {user.Id} from '{user.Status}' to '{to}'.");
      }

      user.Status = to;
      user.UpdatedDate = DateTime.UtcNow;
    }
  }
}
