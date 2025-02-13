using UnityEngine;

public class PlayerIdleState : IPlayerMovementState {
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
    
    this.inputManager.OnMovePerformed += HandleMove;
    this.inputManager.OnJumpPerformed += HandleJump;
    this.inputManager.OnDashPerformed += HandleDash;
    this.inputManager.OnAimPerformed += HandleAim;
    this.inputManager.OnCrouchPerformed += HandleCrouch;

    movementManager.isDashing = false;

    PlayAnimation();
  }

  public void Update() {
    if (!movementManager.isGrounded) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
    }
    if (inputManager.xPosition != 0) {
      stateManager.ChangeMovementState(new PlayerMovingState());
    }
    // if (inputManager.yPosition > 0) {
    //   stateManager.ChangeMovementState(new PlayerAimState());
    // }
    PlayAnimation();
  }

  public void Exit() {
    inputManager.OnMovePerformed -= HandleMove;
    inputManager.OnJumpPerformed -= HandleJump;
    inputManager.OnDashPerformed -= HandleDash;
    inputManager.OnAimPerformed -= HandleAim;
    inputManager.OnCrouchPerformed -= HandleCrouch;
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

  public void PlayAnimation() {
    if (!animatorManager.isCrouchingExit &&
        stateManager.actionState is not PlayerShootingState &&
        stateManager.actionState is not PlayerExShootingState &&
        stateManager.actionState is not PlayerSuperState &&
        stateManager.actionState is not PlayerDeathState) {
      if (inputManager.yPosition > 0) {
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.AimingUp);
        return;
      }
      animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Idle);
    }
  }
}
