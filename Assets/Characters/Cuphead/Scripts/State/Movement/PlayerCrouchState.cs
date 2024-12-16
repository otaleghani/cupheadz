using System;

public class PlayerCrouchState : IPlayerMovementState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;
  private Type previousActionState;

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
    inputManager.OnCrouchCanceled += HandleCrouchCanceled;
    HandleStateAnimation();
  }

  public void UpdateState() {
    HandleStateAnimation();
  }

  public void ExitState() {
    inputManager.OnCrouchCanceled -= HandleCrouchCanceled;
  }

  private void HandleCrouchCanceled() {
    stateManager.ChangeMovementState(new PlayerIdleState());
  }

  private void HandleStateAnimation() {
    if (stateManager.actionState.GetType() != previousActionState) {
      if (stateManager.actionState is not PlayerShootingState) {
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Crouching);
      }
    }
  }
}
