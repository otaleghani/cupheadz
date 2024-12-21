public class PlayerDamagedState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager= animatorManager;

    // set the animation with new system
  }

  public void Update() {
    // update a counter, or wait for the animation to finish
  }

  public void Exit() {}
  public void PlayAnimation() {}
}
