public class PlayerShootingState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private PlayerInputManager.AimDirection previousCoordinate;
  private CupheadWeaponManager weaponManager;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;

    this.inputManager.OnShootCanceled += HandleShootingReleased;
    //this.input
    PlayAnimation();
  }

  public void Update() {
    PlayAnimation();
  }

  public void Exit() {
    inputManager.OnShootCanceled -= HandleShootingReleased;
  }

  private void HandleShootingReleased() {
    stateManager.ChangeActionState(new PlayerNoneState());
  }

  public void PlayAnimation() {
    switch (stateManager.movementState) {
      case PlayerIdleState: 
        if (stateManager.currentShootingState == PlayerStateManager.ShootingState.Aim) {
          animatorManager.ChangeAnimation(animatorManager.shootAimAnimations[PlayerInputManager.CurrentCoordinate]);
        } else {
          animatorManager.ChangeAnimation(animatorManager.shootRecoilAnimations[PlayerInputManager.CurrentCoordinate]);
        }
        previousCoordinate = PlayerInputManager.CurrentCoordinate;
        break;
      case PlayerMovingState:
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.RunningShooting);
        break;

      case PlayerCrouchState:
        if (stateManager.currentShootingState == PlayerStateManager.ShootingState.Aim) {
          animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingShootingAim);
        } else {
          animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.CrouchingShootingRecoil);
        }
        break;

      case PlayerAimState: 
        if (stateManager.currentShootingState == PlayerStateManager.ShootingState.Aim) {
          animatorManager.ChangeAnimation(animatorManager.shootAimAnimations[PlayerInputManager.CurrentCoordinate]);
        } else {
          animatorManager.ChangeAnimation(animatorManager.shootRecoilAnimations[PlayerInputManager.CurrentCoordinate]);
        }
        previousCoordinate = PlayerInputManager.CurrentCoordinate;
        break;
    }
  }
}
