using System;

public class PlayerShootingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private Type previousMovementState;
  private PlayerInputManager.AimDirection previousCoordinate;
  private CupheadWeaponManager weaponManager;

  public void EnterState(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    this.inputManager.OnShootCanceled += HandleShootingReleased;
    HandleStateAnimation();
  }

  public void UpdateState() {
    HandleStateAnimation();
  }

  public void ExitState() {
    inputManager.OnShootCanceled -= HandleShootingReleased;
  }

  private void HandleShootingReleased() {
    stateManager.ChangeActionState(new PlayerNoneState());
  }

  private void HandleStateAnimation() {
    switch (stateManager.movementState) {
      case PlayerIdleState: 
        if (stateManager.currentShootingState == PlayerStateManager.ShootingState.Aim) {
          animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.ShootingAimFront);
        } else {
          animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.ShootingRecoilFront);
        }
        break;
      case PlayerMovingState:
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.RunningShooting);
        previousMovementState = stateManager.movementState.GetType();
        break;

      case PlayerCrouchState:
        if (stateManager.currentShootingState == PlayerStateManager.ShootingState.Aim) {
          animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingShootingAim);
        } else {
          animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingShootingRecoil);
        }
        previousMovementState = stateManager.movementState.GetType();
        break;

      case PlayerAimState: 
        if (stateManager.currentShootingState == PlayerStateManager.ShootingState.Aim) {
          animatorManager.ChangeAnimation(animatorManager.shootAimAnimations[PlayerInputManager.CurrentCoordinate]);
        } else {
          animatorManager.ChangeAnimation(animatorManager.shootRecoilAnimations[PlayerInputManager.CurrentCoordinate]);
        }
        previousMovementState = stateManager.movementState.GetType();
        previousCoordinate = PlayerInputManager.CurrentCoordinate;
        break;
    }
  }
}
