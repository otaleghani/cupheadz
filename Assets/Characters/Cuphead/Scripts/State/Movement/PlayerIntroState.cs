public class PlayerIntroState : IPlayerMovementState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerMovementManager movementManager;
  private PlayerAnimatorManager animatorManager;
  
  public void Enter(
    PlayerStateManager stateManager, 
    PlayerInputManager inputManager, 
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  ) {
    this.stateManager = stateManager;
    this.inputManager = inputManager;
    this.movementManager = movementManager;
    this.animatorManager = animatorManager;
    
    this.animatorManager.OnIntroAnimationEnd += HandleIntroAnimationEnd;
    PlayAnimation();
    movementManager.HoldPosition();
  }

  public void Update() {}
  public void Exit() {
    this.animatorManager.OnIntroAnimationEnd -= HandleIntroAnimationEnd;
  }

  public void PlayAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Intro);
  }

  private void HandleIntroAnimationEnd() {
    movementManager.ReleaseHoldPosition();
    stateManager.ChangeMovementState(new PlayerIdleState());
  }
}
