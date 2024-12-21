using UnityEngine;
public class PlayerSuperState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  //private float counter = 5f;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    PlayAnimation();
  }

  public void Update() {
    //counter -= Time.deltaTime;
    //if (counter <= 0) {
    //  stateManager.ChangeActionState(new PlayerNoneState());
    //}
  }
  public void Exit() {}

  public void PlayAnimation() {}
}
