public class PlayerShootingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;


  public void EnterState(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    inputManager.OnShootCanceled += HandleShootingReleased;
    animatorManager.SetParameterIsShooting();
  }

  public void UpdateState() {}

  public void ExitState() {
    inputManager.OnShootCanceled -= HandleShootingReleased;
    animatorManager.ResetActionParameters();
  }

  private void HandleShootingReleased() {
    stateManager.ChangeActionState(new PlayerNoneState());
  }
}
