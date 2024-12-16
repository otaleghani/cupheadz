public class PlayerParryingState : IPlayerActionState {
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

    HandleStateAnimation();
    // Activate the trigger collider
  }

  public void UpdateState() {}

  public void ExitState() {}

  private void HandleStateAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Parrying);
  }

  public void ParryAnimationFinished() {
    stateManager.ChangeActionState(new PlayerNoneState());
  }
}
