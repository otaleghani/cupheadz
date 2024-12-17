using UnityEngine;
public class PlayerJumpingState : IPlayerMovementState {
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

    this.inputManager.OnJumpCanceled += HandleJumpCanceled;
    this.inputManager.OnDashPerformed += HandleDash;
    this.inputManager.OnJumpPerformed += HandleParry;

    HandleStateAnimation();
    HandleStateMovement();
    
    // Check if you are already in air
    if (this.movementManager.isGrounded) {
      this.movementManager.StartJump();
    }
  }

  public void UpdateState() {
    if (movementManager.isGrounded) {
      if (movementManager.movementDirection != 0) {
        stateManager.ChangeMovementState(new PlayerMovingState());
      } else {
        stateManager.ChangeMovementState(new PlayerIdleState());
      }
      return;
    }
    HandleStateMovement();
  }

  public void ExitState() {
    inputManager.OnJumpCanceled -= HandleJumpCanceled;
    inputManager.OnJumpPerformed -= HandleParry;
  }

  private void HandleStateAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Jumping);
  }

  private void HandleStateMovement() {}

  private void HandleJumpCanceled() {
    movementManager.jumpHoldReleased = true;
    // Reset PlayerMovementManager params related to jumping
    //movementManager.JumpCanceled();
  }
  private void HandleParry() {
    // Here we want to enter the parry state, which is PlayerActionState.
    // Reason why is that you cannot do other actions while parrying

    // See if you are in the radius of a parry
    // than change to this state
    stateManager.ChangeActionState(new PlayerParryingState());
  }

  private void HandleDash() {
    if (!movementManager.isDashingCooldown) {
      stateManager.ChangeMovementState(new PlayerDashingState());
    }
  }
}
