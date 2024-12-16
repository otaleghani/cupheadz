using System;

public class PlayerMovingState : IPlayerMovementState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;
  private Type previousActionState;

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

    this.inputManager.OnMoveCanceled += HandleMoveCanceled;
    this.inputManager.OnJumpPerformed += HandleJump;
    this.inputManager.OnAimPerformed += HandleAim;
    this.inputManager.OnCrouchPerformed += HandleCrouch;
    this.inputManager.OnDashPerformed += HandleDash;

    HandleStateAnimation();
  }

  public void UpdateState() {
    if (!movementManager.isGrounded) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
      return;
    }
    HandleStateAnimation();
    HandleStateMovement();
  }

  public void ExitState() {
    inputManager.OnMoveCanceled -= HandleMoveCanceled;
    inputManager.OnJumpPerformed -= HandleJump;
    inputManager.OnAimPerformed -= HandleAim;
    inputManager.OnCrouchPerformed -= HandleCrouch;
    inputManager.OnDashPerformed -= HandleDash;

    movementManager.Stop();
  }

  public void HandleMoveCanceled() {
    stateManager.ChangeMovementState(new PlayerIdleState());
  }

  // Actions that you can do from the moving state
  public void HandleDash() {
    stateManager.ChangeMovementState(new PlayerDashingState());
  }
  public void HandleJump() {
    stateManager.ChangeMovementState(new PlayerJumpingState());
  }
  private void HandleAim() {
    stateManager.ChangeMovementState(new PlayerAimState());
  }
  private void HandleCrouch() {
    stateManager.ChangeMovementState(new PlayerCrouchState());
  }

  private void HandleStateAnimation() {
    if (stateManager.actionState.GetType() != previousActionState) {
      if (stateManager.actionState is not PlayerShootingState) {
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Running);
      }
    }
  }

  private void HandleStateMovement() {
    movementManager.Move();
  }
}
