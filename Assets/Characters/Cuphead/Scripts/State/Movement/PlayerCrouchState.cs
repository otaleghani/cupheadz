public class PlayerCrouchState : IPlayerMovementState {
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
    inputManager.OnCrouchCanceled += HandleCrouchCanceled;

    animatorManager.SetParameterIsCrouching();
  }

  public void UpdateState() {}

  public void ExitState() {
    inputManager.OnCrouchCanceled -= HandleCrouchCanceled;
    animatorManager.ResetMovementParameters();
  }

  private void HandleCrouchCanceled() {
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
