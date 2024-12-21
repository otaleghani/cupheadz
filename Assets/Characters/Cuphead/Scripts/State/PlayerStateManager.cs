using UnityEngine;

/// <summary>
/// Handles the current state of the character and its changes. The state is divided in movement
/// and action, so that compound states would be easier to pinpoint. This script also hold the
/// player params, like life points and super meter
/// </summary>
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

  //public Collider2D parryCollider;
  public PlayerParryCollision parryCollision;

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    movementManager = GetComponent<PlayerMovementManager>();
    animatorManager = GetComponent<PlayerAnimatorManager>();
    parryCollision = GetComponentInChildren<PlayerParryCollision>();
    parryCollision.DisableCollider();

    movementState = new PlayerIdleState();
    actionState = new PlayerNoneState();
  }

  private void OnEnable() {
    FightSceneStateManager.Instance.OnChangeState += HandleSceneStateChange;
    parryCollision.OnParryCollision += HandleParryCollision;
  }
  private void OnDisable() {
    FightSceneStateManager.Instance.OnChangeState -= HandleSceneStateChange;
    parryCollision.OnParryCollision -= HandleParryCollision;
  }

  private void Start() {
    movementState.Enter(this, inputManager, movementManager, animatorManager);
    actionState.Enter(this, inputManager, movementManager, animatorManager);
    
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
    //movementState.UpdateState();
    //actionState.UpdateState();
  }

  private void FixedUpdate() {
    movementState.Update();
    actionState.Update();
    if (superMeter <= 5) {
      superMeter += superMeterRateOfChange;
    }
    //Debug.Log(actionState);
    //Debug.Log(movementState);
  }

  private void HandleParryCollision(Collider2D collider) {
    IParryable parryableObject = collider.GetComponent<IParryable>();
    if (parryableObject != null) {
      parryableObject.OnParry();
      actionState.PlayAnimation();
      //if (actionState is PlayerParryingState) {
      //  //actionState.
      //}
    }
  }

  /// <summary> 
  /// Methods used to change the states
  /// </summary>
  public void ChangeMovementState(IPlayerMovementState newState) {
    movementState.Exit();
    movementState = newState;
    movementState.Enter(this, inputManager, movementManager, animatorManager);
  }

  public void ChangeActionState(IPlayerActionState newState) {
    actionState.Exit();
    actionState = newState;
    actionState.Enter(this, inputManager, movementManager, animatorManager);
  }

  // Handles parry
  //private void OnTriggerEnter2D(Collider2D collider) {
  //  if (actionState is PlayerParryingState) {
  //    IParryable parryableObject = collider.GetComponent<IParryable>();
  //    if (parryableObject != null) {
  //      parryableObject.OnParry();
  //      // Make small jump
  //    } else {
  //      TakeDamage();
  //    }
  //  }
  //}
  //
  // Handles enemy and bullet collision
  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Enemy") || 
        collision.gameObject.CompareTag("EnemyBullet")) {
      TakeDamage();
      Debug.Log("Collision with enemy");
    }
  }

  /// <summary>
  /// Helper function to take damage. It also changes the Scene state to Lose, so that the scene
  /// object can notify the other Game Objects.
  /// </summary>
  private void TakeDamage() {
    hearts -= 1;
    if (hearts == 0) {
      FightSceneStateManager.Instance.ChangeState(FightSceneStateManager.SceneState.Lose);
    }
  }

  /// <summary>
  /// Function used to sync the state of the scene with the state of the player.
  /// If the scene is "Entry" the player should just stay put and play an animation.
  /// If the scene is "Play" the player should become interactive.
  /// </summary>
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
