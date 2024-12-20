using UnityEngine;

public class PlayerExShootingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  //private float counter = 2f;

  public void EnterState(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    HandleStateAnimation();
    this.animatorManager.OnExShootingAnimationEnd += HandleAnimationEnd;
  }

  public void UpdateState() {
    //counter -= Time.deltaTime;
    //if (counter <= 0) {
    //  stateManager.ChangeActionState(new PlayerNoneState());
    //}
  }

  public void ExitState() {
    this.animatorManager.OnExShootingAnimationEnd -= HandleAnimationEnd;
  }

  private void HandleStateAnimation() {
    if (stateManager.movementState is PlayerJumpingState) {
      animatorManager.ChangeAnimation(
          animatorManager.shootExAirAnimations[PlayerInputManager.CurrentCoordinate]);
    } else {
      animatorManager.ChangeAnimation(
          animatorManager.shootExGroundAnimations[PlayerInputManager.CurrentCoordinate]);
    }
  }

  private void HandleAnimationEnd() {
    // Todo: find a way to listen if you are shooting or not
    stateManager.ChangeActionState(new PlayerNoneState());
  }
}
