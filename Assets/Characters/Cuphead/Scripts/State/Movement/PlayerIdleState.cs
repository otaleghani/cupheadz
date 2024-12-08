using UnityEngine;

public class PlayerIdleState : IPlayerMovementState {
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
    
    this.inputManager.OnMovePerformed += HandleMove;
    this.inputManager.OnJumpPerformed += HandleJump;
    this.inputManager.OnDashPerformed += HandleDash;
    this.inputManager.OnAimPerformed += HandleAim;
    this.inputManager.OnCrouchPerformed += HandleCrouch;

    movementManager.isDashing = false;
  }

  public void UpdateState() {
    if (!movementManager.isGrounded) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
    }
  }

  public void ExitState() {
    inputManager.OnMovePerformed -= HandleMove;
    inputManager.OnJumpPerformed -= HandleJump;
    inputManager.OnDashPerformed -= HandleDash;
    inputManager.OnAimPerformed -= HandleAim;
    inputManager.OnCrouchPerformed -= HandleCrouch;

    animatorManager.ResetMovementParameters();
  }

  private void HandleJump() {
    stateManager.ChangeMovementState(new PlayerJumpingState());
  }

  private void HandleMove(Vector2 movement) {
    if (movement.x != 0) {
      stateManager.ChangeMovementState(new PlayerMovingState());
    }
  }

  private void HandleDash() {
    if (!movementManager.isDashingCooldown) {
      stateManager.ChangeMovementState(new PlayerDashingState());
    }
  }

  private void HandleAim() {
    stateManager.ChangeMovementState(new PlayerAimState());
  }

  private void HandleCrouch() {
    stateManager.ChangeMovementState(new PlayerCrouchState());
  }
}
