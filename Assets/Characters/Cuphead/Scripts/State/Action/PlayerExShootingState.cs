public class PlayerExShootingState : IPlayerActionState {
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

    this.animatorManager.OnExShootingAnimationEnd += HandleAnimationEnd;
    this.stateManager.ChangeMovementState(new PlayerLockedState());
    HandleStateAnimation();
  }

  public void UpdateState() {}

  public void ExitState() {
    this.animatorManager.OnExShootingAnimationEnd -= HandleAnimationEnd;
  }

  private void HandleStateAnimation() {
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
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
