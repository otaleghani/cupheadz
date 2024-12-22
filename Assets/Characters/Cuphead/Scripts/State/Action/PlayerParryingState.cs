using UnityEngine;
public class PlayerParryingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;
  private int parryFreezeFrames = 10;
  private bool isParryPerformed = false;

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

    // Activate the parrying collider
    //parryWindow = 10;
    this.stateManager.parryCollision.EnableCollider();
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Parrying);
    this.animatorManager.OnParryAnimationEnd += HandleParryAnimationEnd;
  }

  public void Update() {
    if (isParryPerformed) {
      // todo: add pink sparkle
      movementManager.HoldPosition();
      animatorManager.Pause();
      parryFreezeFrames -= 1;
      if (parryFreezeFrames == 0) {
        movementManager.ReleaseHoldPosition();
        movementManager.StartJump();
        movementManager.jumpHoldReleased = false;
        animatorManager.Resume();
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Parrying);
        isParryPerformed = false;
      }
    }
  }

  public void Exit() {
    this.stateManager.parryCollision.DisableCollider();
    this.animatorManager.OnParryAnimationEnd -= HandleParryAnimationEnd;
  }

  public void PlayAnimation() {
    isParryPerformed = true;
  }

  public void HandleParryAnimationEnd() {
    stateManager.movementState.PlayAnimation();
    stateManager.ChangeActionState(new PlayerNoneState());
  }
}
