namespace ProductApp.Models
{
  // The set of states a User account can be in. This is the "states" half of the FSM
  // (the other half — which transitions between states are legal — lives in StateMachine/UserStateMachine.cs).
  public enum UserStatus
  {
    PendingVerification,
    Active,
    Suspended,
    Deactivated
  }
}
