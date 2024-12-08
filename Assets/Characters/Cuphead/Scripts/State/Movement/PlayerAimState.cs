public class PlayerAimState : IPlayerMovementState {
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

    inputManager.OnAimCanceled += HandleAimCanceled;
    animatorManager.SetParameterIsAiming();
  }

  public void UpdateState() {}

  public void ExitState() {
    inputManager.OnAimCanceled -= HandleAimCanceled;
    animatorManager.ResetMovementParameters();
  }

  private void HandleAimCanceled() {
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
