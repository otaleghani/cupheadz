public class PlayerSuperState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private PlayerMovementManager movementManager;
  private CupheadSuperManager superManager;

  public void Enter(
    PlayerStateManager stateManager,
    PlayerInputManager inputManager,
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.animatorManager = animatorManager;
    this.movementManager = movementManager;
    this.superManager = stateManager.GetComponentInChildren<CupheadSuperManager>();

    this.animatorManager.OnSuperAnimationEnd += HandleAnimationEnd;

    PlayAnimation();
    this.superManager.UseSuper();
  }

  public void Update() {
  }
  public void Exit() {
    movementManager.ReleaseHoldPosition();
    animatorManager.OnSuperAnimationEnd -= HandleAnimationEnd;
  }

  public void PlayAnimation() {
    switch (superManager.equippedSuper)
    {
      case GameData.Super.EnergyBeam:
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.SuperEnergyBeam);
        break;
      case GameData.Super.Invincibility:
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.SuperInvincibility);
        break;
      case GameData.Super.GiantGhost:
        animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.SuperGiantGhost);
        break;
    }
  }

  private void HandleAnimationEnd() {
    stateManager.ChangeActionState(new PlayerNoneState());
    if (movementManager.isJumping) {
      stateManager.ChangeMovementState(new PlayerJumpingState());
      return;
    }
    if (inputManager.xPosition != 0) {
      stateManager.ChangeMovementState(new PlayerMovingState());
      return;
    }
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
