public class PlayerMovingState : IPlayerMovementState {
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

    inputManager.OnMoveCanceled += HandleMoveCanceled;
    inputManager.OnJumpPerformed += HandleJump;
    inputManager.OnLockPerformed += HandleLock;
    inputManager.OnCrouchPerformed += HandleCrouch;

    animatorManager.SetParameterIsMoving();
  }

  public void UpdateState() {
    if (!movementManager.isGrounded) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
    }
  }

  public void ExitState() {
    inputManager.OnMoveCanceled -= HandleJump;
    inputManager.OnJumpPerformed -= HandleJump;
    inputManager.OnLockPerformed -= HandleJump;

    animatorManager.ResetMovementParameters();
  }

  public void HandleMoveCanceled() {
    stateManager.ChangeMovementState(new PlayerIdleState());
  }

  // Actions that you can do from the moving state
  public void HandleDash() {}
  public void HandleJump() {
    stateManager.ChangeMovementState(new PlayerJumpingState());
  }
  private void HandleLock() {
    stateManager.ChangeMovementState(new PlayerLockState());
  }
  private void HandleCrouch() {
    stateManager.ChangeMovementState(new PlayerCrouchState());
  }
}
