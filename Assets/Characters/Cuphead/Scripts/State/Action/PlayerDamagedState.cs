public class PlayerDamagedState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private PlayerMovementManager movementManager;
  private bool isFacingRight;

  public PlayerDamagedState(bool isFacingRight) {
    this.isFacingRight = isFacingRight;
  }

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;
    this.movementManager = movementManager;
    this.animatorManager.OnDamageAnimationEnd += HandleAnimationEnd;

    //this.movementManager.isFacingRight = !isFacingRight;
    this.movementManager.ExRecoil();
    this.stateManager.ChangeMovementState(new PlayerDamagedMovementState());

    PlayAnimation();
  }

  public void Update() {}

  public void Exit() {
    this.animatorManager.OnDamageAnimationEnd += HandleAnimationEnd;
  }
  public void PlayAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Damage);
  }

  private void HandleAnimationEnd() {
    stateManager.ChangeActionState(new PlayerNoneState());
    if (movementManager.isJumping) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
      return;
    }
    if (inputManager.xPosition != 0) {
      stateManager.ChangeMovementState(new PlayerMovingState());
      return;
    }
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
