using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
  [Header("Player stats")]
  public int hearts = 3;
  public int superMeter = 0;

  private PlayerMovementManager movementManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public IPlayerMovementState movementState;
  public IPlayerActionState actionState;
  public enum ShootingState { Aim, Recoil };
  public ShootingState currentShootingState = ShootingState.Aim;

  // I could create here a "state boolean"
  // Something like "isShooting"

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    movementManager = GetComponent<PlayerMovementManager>();
    animatorManager = GetComponent<PlayerAnimatorManager>();

    movementState = new PlayerIdleState();
    actionState = new PlayerNoneState();
  }

  void OnEnable() {
    FightSceneStateManager.Instance.OnChangeState += HandleSceneStateChange;
  }
  void OnDisable() {
    FightSceneStateManager.Instance.OnChangeState -= HandleSceneStateChange;
  }

  void Start() {
    movementState.EnterState(this, inputManager, movementManager, animatorManager);
    actionState.EnterState(this, inputManager, animatorManager);
  }

  void Update() {
    movementState.UpdateState();
    actionState.UpdateState();
    Debug.Log(movementState);
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

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet")) {
      hearts -= 1;
      if (hearts == 0) {
        FightSceneStateManager.Instance.ChangeState(FightSceneStateManager.SceneState.Lose);
      }
    }
  }

  private void HandleSceneStateChange(FightSceneStateManager.SceneState currentState) {
    switch (currentState) {
      case FightSceneStateManager.SceneState.Entry:
        ChangeMovementState(new PlayerIdleState());
        //ChangeActionState(new PlayerEntryState());
        break;
      case FightSceneStateManager.SceneState.Win:
        ChangeMovementState(new PlayerIdleState());
        ChangeActionState(new PlayerNoneState());
        // Disable the colliders
        break;
      case FightSceneStateManager.SceneState.Lose:
        ChangeActionState(new PlayerNoneState());
        ChangeMovementState(new PlayerDeathState());
        break;
      case FightSceneStateManager.SceneState.Play:
        ChangeMovementState(new PlayerIdleState());
        ChangeActionState(new PlayerNoneState());
        break;
      case FightSceneStateManager.SceneState.Exit:
        ChangeMovementState(new PlayerIdleState());
        //ChangeActionState(new PlayerExitState());
        break;
      default:
        break;
    }
  }
}
