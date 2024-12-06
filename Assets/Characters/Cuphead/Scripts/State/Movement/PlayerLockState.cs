public class PlayerLockState : IPlayerMovementState {
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
    this.animatorManager = animatorManager;

    inputManager.OnLockCanceled += HandleLockReleased;
    // animatorManager.SetParameterIsLocked();
  }

  public void UpdateState() {}

  public void ExitState() {
    inputManager.OnLockCanceled -= HandleLockReleased;
    animatorManager.ResetMovementParameters();
  }

  private void HandleLockReleased() {
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
