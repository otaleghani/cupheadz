// Here the player is not shooting
public class PlayerNoneState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    this.inputManager.OnShootPerformed += HandleShooting;
    this.inputManager.OnShootEXPerformed += HandleShootingEx;
  }

  public void Update() {}

  public void Exit() {
    inputManager.OnShootPerformed -= HandleShooting;
    inputManager.OnShootCanceled -= HandleShootingEx;
  }

  public void PlayAnimation() {}

  private void HandleShooting() {
    stateManager.ChangeActionState(new PlayerShootingState());
  }
  private void HandleShootingEx() {
    // Here I need to actually calculate the shooting data
    if (stateManager.superMeter >= 5f) {
      stateManager.ChangeActionState(new PlayerSuperState());
      stateManager.ChangeMovementState(new PlayerLockedState());
    } else if (stateManager.superMeter >= 1f) {
      stateManager.ChangeActionState(new PlayerExShootingState());
      stateManager.ChangeMovementState(new PlayerLockedState());
    }
  }
}
