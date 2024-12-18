using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
  [Header("Player stats")]
  public int hearts = 3;
  public float superMeter = 4f;
  public float superMeterRateOfChange = 0f;

  private PlayerMovementManager movementManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public IPlayerMovementState movementState;
  public IPlayerActionState actionState;
  public enum ShootingState { Aim, Recoil };
  public ShootingState currentShootingState = ShootingState.Aim;

  public Collider2D parryCollider;

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    movementManager = GetComponent<PlayerMovementManager>();
    animatorManager = GetComponent<PlayerAnimatorManager>();

    // Create new base states
    movementState = new PlayerIdleState();
    actionState = new PlayerNoneState();

    // Disables parry collider on startup
    if (parryCollider != null) {
      parryCollider.enabled = false;
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
    
    // Handles charms
    if (CupheadCharmsManager.Instance.equippedCharm[GameData.Charm.Heart]) {
      // Todo: Add negative damage taken modifier
      hearts += 1;
    }
    if (CupheadCharmsManager.Instance.equippedCharm[GameData.Charm.TwinHeart]) {
      // Todo: Add negative damage taken modifier
      hearts += 2;
    }
    if (CupheadCharmsManager.Instance.equippedCharm[GameData.Charm.Coffee]) {
      superMeterRateOfChange = 0.005f;
    }
  }

  private void Update() {
    movementState.UpdateState();
    actionState.UpdateState();
  }

  private void FixedUpdate() {
    if (superMeter <= 5) {
      superMeter += superMeterRateOfChange;
    }
    Debug.Log(actionState.GetType());
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
    if (collision.gameObject.CompareTag("Enemy") || 
        collision.gameObject.CompareTag("EnemyBullet")) {
      TakeDamage();
    }

    if (actionState is PlayerParryingState) {
      IParryable parryableObject = collision.GetComponent<IParryable>();
      if (parryableObject != null) {
        parryableObject.OnParry();
        // Make a small jump 
      } 
    }
  }

  private void TakeDamage() {
    hearts -= 1;
    if (hearts == 0) {
      FightSceneStateManager.Instance.ChangeState(FightSceneStateManager.SceneState.Lose);
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
