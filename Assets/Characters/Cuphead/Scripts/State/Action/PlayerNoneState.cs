// Here the player is not shooting
public class PlayerNoneState : IPlayerActionState {
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

    inputManager.OnShootPerformed += HandleShooting;
    //animatorManager.SetParameterIsShooting();
  }

  public void UpdateState() {}

  public void ExitState() {
    inputManager.OnShootPerformed -= HandleShooting;
    animatorManager.ResetActionParameters();
  }

  private void HandleShooting() {
    stateManager.ChangeActionState(new PlayerShootingState());
  }
}
