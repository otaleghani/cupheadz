public class PlayerJumpingState : IPlayerMovementState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;

  public void EnterState(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.movementManager = movementManager;
    this.animatorManager= animatorManager;

    animatorManager.SetParameterIsJumping();
  }

  public void UpdateState() {
    if (movementManager.isGrounded) {
      stateManager.ChangeMovementState(new PlayerIdleState());
    }
  }

  public void ExitState() {
    animatorManager.ResetMovementParameters();
  }
}
