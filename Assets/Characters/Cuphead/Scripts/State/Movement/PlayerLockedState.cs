/// <summary>
/// State used to stop the player from moving. Used usually in case of Supers or ExShooting
/// </summary>
public class PlayerLockedState : IPlayerMovementState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private PlayerMovementManager movementManager;

  public void EnterState(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;
    this.movementManager = movementManager;

    this.movementManager.HoldPosition();
  }

  public void UpdateState() {}

  public void ExitState() {
    movementManager.ReleaseHoldPosition();
  }
}
