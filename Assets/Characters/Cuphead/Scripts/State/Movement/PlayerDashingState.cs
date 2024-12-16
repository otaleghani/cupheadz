public class PlayerDashingState : IPlayerMovementState {
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
    HandleStateAnimation();
  }

  public void UpdateState() {
    if (!movementManager.isDashing) {
      stateManager.ChangeMovementState(new PlayerIdleState());
    }
    HandleStateMovement();
  }

  public void ExitState() {}

  private void HandleStateAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Dashing);
  }

  private void HandleStateMovement() {
    movementManager.Dash();
  }

  public void DashAnimationFinished() {
    // Reset PlayerMovementManager params related to dashing
    movementManager.EndDashMovement();

    // Understand which would be the next state and change it
    if (!movementManager.isGrounded) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
      return;
    }
    if (movementManager.isMoving) {
      stateManager.ChangeMovementState(new PlayerMovingState());
      return;
    }
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
