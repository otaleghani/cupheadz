public interface IPlayerActionState {
  void EnterState(
    PlayerStateManager stateManager, 
    PlayerInputManager inputManager,
    PlayerAnimatorManager animatorManager
  );
  void UpdateState();
  void ExitState();
}
