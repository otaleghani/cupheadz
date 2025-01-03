public class PlayerExShootingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.movementManager = movementManager;
    this.animatorManager = animatorManager;

    this.animatorManager.OnExShootingAnimationEnd += HandleAnimationEnd;
    this.stateManager.ChangeMovementState(new PlayerLockedState());
    PlayAnimation();
    this.stateManager.RemoveToSuperMeter(1f);
  }

  public void Update() {}

  public void Exit() {
    this.animatorManager.OnExShootingAnimationEnd -= HandleAnimationEnd;
  }

  public void PlayAnimation() {
    if (!movementManager.isGrounded) {
      animatorManager.ChangeAnimation(
        animatorManager.shootExAirAnimations[PlayerInputManager.CurrentCoordinate]);
    } else {
      animatorManager.ChangeAnimation(
        animatorManager.shootExGroundAnimations[PlayerInputManager.CurrentCoordinate]);
    }
  }

  private void HandleAnimationEnd() {
    // Todo: find a way to listen if you are shooting or not
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
