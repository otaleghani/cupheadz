public class PlayerCrouchState : IPlayerMovementState {
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

    this.inputManager.OnCrouchCanceled += HandleCrouchCanceled;
    this.inputManager.OnJumpPerformed += HandleDropOff;
    this.inputManager.OnShootPerformed += HandleShooting;
    this.animatorManager.isCrouchingEnter = true;
    PlayAnimation();
    this.movementManager.MoveStop();
  }

  public void Update() {
    if (!movementManager.isGrounded) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
      return;
    }
  }

  public void Exit() {
    inputManager.OnCrouchCanceled -= HandleCrouchCanceled;
    inputManager.OnJumpPerformed -= HandleDropOff;
    inputManager.OnShootPerformed -= HandleShooting;
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
      //movementManager.isGrounded = false;
    }
    // I don't know if you would actually need to do this
    //stateManager.ChangeMovementState(new PlayerJumpingState());
  }

  private void HandleShooting() {
    if (stateManager.actionState is not PlayerShootingState) {
      stateManager.ChangeActionState(new PlayerShootingState());
    }
  }

  public void PlayAnimation() {
    if (animatorManager.isCrouchingEnter) {
      animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Crouching);
      return;
    }
    if (stateManager.actionState is not PlayerShootingState) {
      animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingIdle);
    }
  }
}
