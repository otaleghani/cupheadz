
// Here the player is not shooting
public class PlayerDeathState : IPlayerMovementState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private PlayerMovementManager movementManager;

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

    this.animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Dead);
  }

  public void UpdateState() {}
  public void ExitState() {}
}
