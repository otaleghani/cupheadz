public interface IPlayerMovementState {
  void EnterState(
    PlayerStateManager stateManager, 
    PlayerInputManager inputManager, 
    PlayerMovementManager movementManager,
    PlayerAnimatorManager animatorManager
  );
  void UpdateState();
  void ExitState();
}
