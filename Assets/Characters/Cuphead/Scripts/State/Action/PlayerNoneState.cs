// Here the player is not shooting
public class PlayerNoneState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public void EnterState(
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

  public void UpdateState() {}

  public void ExitState() {
    inputManager.OnShootPerformed -= HandleShooting;
    inputManager.OnShootCanceled -= HandleShootingEx;
  }

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
