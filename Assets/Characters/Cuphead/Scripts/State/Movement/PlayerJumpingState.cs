using UnityEngine;

public class PlayerJumpingState : IPlayerMovementState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;
  private bool canParry;

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

    this.inputManager.OnJumpCanceled += HandleJumpCanceled;
    this.inputManager.OnDashPerformed += HandleDash;
    this.inputManager.OnJumpPerformed += HandleParry;

    PlayAnimation();
    HandleStateMovement();
    
    // Check if you are already in air
    if (this.movementManager.isGrounded) {
      this.movementManager.StartJump();
    }
    canParry = true;
  }

  public void Update() {
    if (movementManager.isGrounded) {
      if (inputManager.xPosition != 0) {
        stateManager.ChangeMovementState(new PlayerMovingState());
      } else {
        stateManager.ChangeMovementState(new PlayerIdleState());
      }
      return;
    }
  }

  public void Exit() {
    inputManager.OnJumpCanceled -= HandleJumpCanceled;
    inputManager.OnJumpPerformed -= HandleParry;
    inputManager.OnDashPerformed -= HandleDash;
  }

  public void PlayAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Jumping);
  }

  private void HandleStateMovement() {}

  private void HandleJumpCanceled() {
    movementManager.jumpHoldReleased = true;
  }

  private void HandleParry() {
    // Here we want to enter the parry state, which is PlayerActionState.
    // Reason why is that you cannot do other actions while parrying

    // See if you are in the radius of a parry
    // than change to this state
    if (canParry && stateManager.actionState is not PlayerParryingState) {
      stateManager.ChangeActionState(new PlayerParryingState());
      //canParry = false;
    }
  }

  private void HandleDash() {
    if (!movementManager.isDashingCooldown) {
      stateManager.ChangeMovementState(new PlayerDashingState());
    }
  }
}
