using UnityEngine;

public class PlayerExShootingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private float counter = 2f;

  public void EnterState(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    HandleStateAnimation();
  }

  public void UpdateState() {
    counter -= Time.deltaTime;
    if (counter <= 0) {
      stateManager.ChangeActionState(new PlayerNoneState());
    }
  }
  public void ExitState() {}

  private void HandleStateAnimation() {
    // You actually have to get if you are on ground or not
  }
}
