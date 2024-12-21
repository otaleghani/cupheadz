public interface IPlayerMovementState {
  void Enter(
    PlayerStateManager stateManager, 
    PlayerInputManager inputManager, 
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  );
  void Update();
  void Exit();
  void PlayAnimation();
}
