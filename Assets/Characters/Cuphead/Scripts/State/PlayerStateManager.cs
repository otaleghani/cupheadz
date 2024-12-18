using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
  [Header("Player stats")]
  public int hearts = 3;
  public float superMeter = 0f;
  public float superMeterRateOfChange = 0f;

  private PlayerMovementManager movementManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public IPlayerMovementState movementState;
  public IPlayerActionState actionState;
  public enum ShootingState { Aim, Recoil };
  public ShootingState currentShootingState = ShootingState.Aim;

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    movementManager = GetComponent<PlayerMovementManager>();
    animatorManager = GetComponent<PlayerAnimatorManager>();

    // Create new base states
    movementState = new PlayerIdleState();
    actionState = new PlayerNoneState();

    // Handles charms
    if (CupheadCharmsManager.Instance.equippedCharm[GameData.Charm.Heart]) {
      // Todo: Add negative damage modifier
      hearts += 1;
    }
    if (CupheadCharmsManager.Instance.equippedCharm[GameData.Charm.TwinHeart]) {
      // Todo: Add negative damage modifier
      hearts += 2;
    }
    if (CupheadCharmsManager.Instance.equippedCharm[GameData.Charm.Coffee]) {
      superMeterRateOfChange = 0.005f;
    }
  }

  private void OnEnable() {
    FightSceneStateManager.Instance.OnChangeState += HandleSceneStateChange;
  }
  private void OnDisable() {
    FightSceneStateManager.Instance.OnChangeState -= HandleSceneStateChange;
  }

  private void Start() {
    movementState.EnterState(this, inputManager, movementManager, animatorManager);
    actionState.EnterState(this, inputManager, animatorManager);
  }

  private void Update() {
    movementState.UpdateState();
    actionState.UpdateState();
  }

  private void FixedUpdate() {
    if (superMeter <= 5) {
      superMeter += superMeterRateOfChange;
    }
  }

  //public AddToSuperMeter(float number) {}

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
