using UnityEngine;
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

    // this.inputManager.OnShootPerformed += HandleShooting;
    this.inputManager.OnShootEXPerformed += HandleShootingEx;

    PlayAnimation();
  }

  public void Update() {
    PlayAnimation();
    
    if (stateManager.IsShooting && stateManager.actionState is not PlayerShootingState) {
      stateManager.ChangeActionState(new PlayerShootingState());
    }
  }

  public void Exit() {
    // inputManager.OnShootPerformed -= HandleShooting;
    inputManager.OnShootEXPerformed -= HandleShootingEx;
  }

  public void PlayAnimation() {
    if (stateManager.movementState is PlayerDeathState) return;
    
    if (stateManager.movementState is PlayerCrouchState) {
      animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingIdle);
    }
    // if (stateManager.movementState is PlayerIdleState) {
    //   animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Idle);
    // }
    if (stateManager.movementState is PlayerMovingState) {
      animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Running);
    }
  }

  // private void HandleShooting() {
  //   stateManager.ChangeActionState(new PlayerShootingState());
  // }
  private void HandleShootingEx() {
    if (stateManager.superMeter >= 5f) {
      stateManager.ChangeActionState(new PlayerSuperState());
      stateManager.ChangeMovementState(new PlayerLockedState());
      return;
    } else if (stateManager.superMeter >= 1f) {
      stateManager.ChangeActionState(new PlayerExShootingState());
      stateManager.ChangeMovementState(new PlayerLockedState());
    }
  }
}
