using System;

public class PlayerCrouchState : IPlayerMovementState {
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

    this.inputManager.OnCrouchCanceled += HandleCrouchCanceled;
    this.inputManager.OnJumpPerformed += HandleDropOff;
    this.animatorManager.isCrouchingEnter = true;
    HandleStateAnimation();
    this.movementManager.MoveStop();
  }

  public void UpdateState() {

  }

  public void ExitState() {
    inputManager.OnCrouchCanceled -= HandleCrouchCanceled;
    inputManager.OnJumpPerformed -= HandleDropOff;
  }

  private void HandleCrouchCanceled() {
    if (inputManager.xPosition == 0) {
      animatorManager.isCrouchingExit = true;
      animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingExit);
      stateManager.ChangeMovementState(new PlayerIdleState());
    } else {
      stateManager.ChangeMovementState(new PlayerMovingState());
    }
  }

  private void HandleDropOff() {
    IDropOffGround dropOffGround = movementManager.currentGround.GetComponent<IDropOffGround>();
    if (dropOffGround != null) {
      dropOffGround.DeactivateCollider(1f);
    }
    // I don't know if you would actually need to do this
    stateManager.ChangeMovementState(new PlayerJumpingState());
  }

  private void HandleStateAnimation() {
    if (animatorManager.isCrouchingEnter) {
      animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Crouching);
      return;
    }
    if (stateManager.actionState.GetType() != previousActionState) {
      if (stateManager.actionState is not PlayerShootingState) {
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingIdle);
      }
    }
  }
}
