using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
  private PlayerMovementManager movementManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public IPlayerMovementState movementState;
  public IPlayerActionState actionState;

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    movementManager = GetComponent<PlayerMovementManager>();
    animatorManager = GetComponent<PlayerAnimatorManager>();

    movementState = new PlayerIdleState();
    actionState = new PlayerNoneState();
  }

  void Start() {
    movementState.EnterState(this, inputManager, movementManager, animatorManager);
    actionState.EnterState(this, inputManager, animatorManager);
  }

  void FixedUpdate() {
    //Debug.Log(movementState.GetType());
    movementState.UpdateState();
    actionState.UpdateState();
  }

  public void ChangeMovementState(IPlayerMovementState newState) {
    movementState.ExitState();
    movementState = newState;
    movementState.EnterState(this, inputManager, movementManager, animatorManager);
  }

  public void ChangeActionState(IPlayerActionState newState) {
    actionState.ExitState();
    actionState = newState;
    actionState.EnterState(this, inputManager, animatorManager);
  }
}
