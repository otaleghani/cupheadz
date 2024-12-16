public class PlayerDamagedState : IPlayerActionState {
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
    this.animatorManager= animatorManager;

    // set the animation with new system
  }

  public void UpdateState() {
    // update a counter, or wait for the animation to finish
  }

  public void ExitState() {}
}
