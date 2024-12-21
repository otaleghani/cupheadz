public class PlayerDashingState : IPlayerMovementState {
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
    this.animatorManager.OnDashingAnimationEnd += DashAnimationFinished;
    PlayAnimation();
    movementManager.isDashing = true;
    movementManager.HoldYPosition();
  }

  public void Update() {
    if (!movementManager.isDashing) {
      stateManager.ChangeMovementState(new PlayerIdleState());
    }
    HandleStateMovement();
  }

  public void Exit() {
    this.animatorManager.OnDashingAnimationEnd -= DashAnimationFinished;
    movementManager.ReleaseHoldPosition();
  }

  public void PlayAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Dashing);
  }

  private void HandleStateMovement() {
    //movementManager.Dash();
  }

  private void DashAnimationFinished() {
    // Reset PlayerMovementManager params related to dashing
    movementManager.EndDash();

    // Understand which would be the next state and change it
    if (!movementManager.isGrounded) {
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
