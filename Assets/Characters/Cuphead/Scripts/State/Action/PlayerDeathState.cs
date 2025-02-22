using UnityEngine.SceneManagement;

public class PlayerDeathState : IPlayerActionState {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;
  private PlayerMovementManager movementManager;

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
    this.animatorManager.OnDeathAnimationEnd += HandleAnimationEnd;

    PlayAnimation();
    this.movementManager.HoldPosition();
  }

  public void Update() {}
  public void Exit() {}
  public void PlayAnimation() {
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.Death);
  }

  private void HandleAnimationEnd() {
    FightSceneStateManager.Instance.ActivateDeathCard(stateManager.LastContact);
    animatorManager.ChangeAnimation(PlayerAnimatorManager.PlayerAnimations.None);
  }
}
