using System;

public class PlayerAimState : IPlayerMovementState {
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

    this.inputManager.OnAimCanceled += HandleAimCanceled;
    HandleStateAnimation();
  }

  public void UpdateState() {
    HandleStateAnimation();
    // In the Aiming state you cannot move the character
    // Does the jump override this behaviour?
  }

  public void ExitState() {
    inputManager.OnAimCanceled -= HandleAimCanceled;
  }

  private void HandleAimCanceled() {
    if (inputManager.xPosition == 0) {
      stateManager.ChangeMovementState(new PlayerIdleState());
    } else {
      stateManager.ChangeMovementState(new PlayerMovingState());
    }
  }

  private void HandleStateAnimation() {
    if (stateManager.actionState.GetType() != previousActionState) {
      if (stateManager.actionState is not PlayerShootingState) {
        animatorManager.ChangeAnimation(animatorManager.aimAnimations[PlayerInputManager.CurrentCoordinate]);
      }
    }
  }
}
