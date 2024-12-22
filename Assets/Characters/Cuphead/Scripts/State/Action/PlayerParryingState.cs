using UnityEngine;
public class PlayerParryingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;
  private int parryFreezeFrames;
  private bool isParryPerformed;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;
    this.movementManager = movementManager;

    parryFreezeFrames = 10;
    isParryPerformed = false;
    this.stateManager.parryCollision.EnableCollider();
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Parrying);
    this.animatorManager.OnParryAnimationEnd += HandleParryAnimationEnd;
  }

  public void Update() {
    if (isParryPerformed) {
      // todo: add pink sparkle
      Lock();
      int frameOfImpact = animatorManager.GetAnimationCurrentFrame();
      animatorManager.ChangeAnimationFromFrame(PlayerAnimatorManager.PlayerAnimations.ParryingPink, frameOfImpact);
      parryFreezeFrames -= 1;
      if (parryFreezeFrames == 0) {
        Unlock();
        animatorManager.ChangeAnimationFromFrame(PlayerAnimatorManager.PlayerAnimations.Parrying, frameOfImpact);
        movementManager.StartJump();
        //movementManager.
        movementManager.jumpHoldReleased = false;
        isParryPerformed = false;
      }
    }
    
    if (movementManager.isGrounded) {
      // If it's grounded it means that the player stopped the jumping
      stateManager.ChangeActionState(new PlayerNoneState());
    }
  }

  public void Exit() {
    stateManager.parryCollision.DisableCollider();
    animatorManager.OnParryAnimationEnd -= HandleParryAnimationEnd;
  }

  public void PlayAnimation() {
    isParryPerformed = true;
  }


  public void HandleParryAnimationEnd() {
    stateManager.movementState.PlayAnimation();
    isParryPerformed = false;
    Unlock();
    stateManager.ChangeActionState(new PlayerNoneState());
  }

  private void Lock() {
    movementManager.HoldPosition();
    animatorManager.Pause();
  }
  private void Unlock() {
    movementManager.ReleaseHoldPosition();
    animatorManager.Resume();
  }
}
