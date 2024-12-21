using UnityEngine;
public class PlayerParryingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private int parryWindow;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    // Activate the parrying collider
    //parryWindow = 10;
    this.stateManager.parryCollision.EnableCollider();
    PlayAnimation();
    this.animatorManager.OnParryAnimationEnd += HandleParryAnimationEnd;
  }

  public void Update() {
    //parryWindow -= 1;
    //if (parryWindow <= 0) {
    //  stateManager.ChangeActionState(new PlayerNoneState());
    //}
  }

  public void Exit() {
    this.stateManager.parryCollision.DisableCollider();
    this.animatorManager.OnParryAnimationEnd -= HandleParryAnimationEnd;
  }

  public void PlayAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Parrying);
  }

  public void HandleParryAnimationEnd() {
    stateManager.movementState.PlayAnimation();
    stateManager.ChangeActionState(new PlayerNoneState());
  }
}
