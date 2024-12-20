public class PlayerParryingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public void EnterState(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    HandleStateAnimation();
    // Activate the parrying collider
    this.stateManager.parryCollider.enabled = true;
  }

  public void UpdateState() {}

  public void ExitState() {
    // Disables parry collider 
    stateManager.parryCollider.enabled = true;
  }

  private void HandleStateAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Parrying);
  }

  public void ParryAnimationFinished() {
    stateManager.ChangeActionState(new PlayerNoneState());
  }
}
