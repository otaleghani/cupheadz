using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Handles the current state of the character and its changes. The state is divided in movement
/// and action, so that compound states would be easier to pinpoint. This script also hold the
/// player params, like life points and super meter
/// </summary>
public class PlayerStateManager : MonoBehaviour {
  public static PlayerStateManager instance;

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
  public bool IsShooting = false; // Used to signal if the player is pressing the shoot button or not

  public event Action<int> OnPlayerHealthChange;
  public event Action<float> OnPlayerSuperMeterChange;

  //public Collider2D parryCollider;
  public PlayerParryCollision parryCollision;

  // Used to change the material in case of super-invicibility
  public SpriteRenderer spriteRenderer;
  private Material invincibilityMaterial;
  private Material defaultMaterial;
  private float invincibilityTime = 10f;
  private bool isInvincible = false;
  private bool isPaused = false;
  public GameObject pauseUI;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else {
      Debug.LogWarning("Found more than one instances of PlayerStateManager");
    }
    inputManager = GetComponent<PlayerInputManager>();
    movementManager = GetComponent<PlayerMovementManager>();
    animatorManager = GetComponent<PlayerAnimatorManager>();

    spriteRenderer = GetComponent<SpriteRenderer>();
    invincibilityMaterial = Resources.Load<Material>("DuotoneMaterial");
    defaultMaterial = spriteRenderer.material;

    parryCollision = GetComponentInChildren<PlayerParryCollision>();

    movementState = new PlayerIntroState();
    actionState = new PlayerNoneState();
  }


  private void OnEnable() {
    FightSceneStateManager.Instance.OnChangeState += HandleSceneStateChange;
    parryCollision.OnParryCollision += HandleParryCollision;
    inputManager.OnPausePerformed += HandlePause;
    inputManager.OnShootPerformed += HandleShootPerformed;
    inputManager.OnShootCanceled += HandleShootCanceled;
  }
  private void OnDisable() {
    FightSceneStateManager.Instance.OnChangeState -= HandleSceneStateChange;
    parryCollision.OnParryCollision -= HandleParryCollision;
    inputManager.OnPausePerformed -= HandlePause;
    inputManager.OnShootPerformed -= HandleShootPerformed;
    inputManager.OnShootCanceled -= HandleShootCanceled;
  }

  private void HandlePause() {
    isPaused = !isPaused;
    if (isPaused) {
      Pause();
    } else {
      Resume();
    }
  }

  private void Pause() {
    pauseUI.SetActive(true);
    Time.timeScale = 0f;
    inputManager.SwitchToUi();
    //inputManager.SwitchActionMap("UI");
  }
  public void Resume() {
    pauseUI.SetActive(false);
    Time.timeScale = 1f;
    inputManager.SwitchToPlayer();
    //inputManager.SwitchActionMap("Player");
  }

  private void Start() {
    inputManager.SwitchToPlayer();
    parryCollision.DisableCollider();

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

    OnPlayerHealthChange?.Invoke(hearts);
    OnPlayerSuperMeterChange?.Invoke(superMeter);
  }

  private void FixedUpdate() {
    movementState.Update();
    actionState.Update();
    if (superMeter < 5 && superMeterRateOfChange != 0f) {
      AddToSuperMeter(superMeterRateOfChange);
    }
    Debug.Log(actionState);
    Debug.Log(movementState);
  }

  private void HandleParryCollision(Collider2D collider) {
    IParryable parryableObject = collider.GetComponent<IParryable>();
    if (parryableObject != null) {
      parryableObject.OnParry();
      actionState.PlayAnimation();
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
  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet")) {
      TakeDamage(CalculateApproximateContactPoint(other).x > 0.5 ? true : false);
      StartCoroutine(TemporaryInvulnerability(gameObject));
    }
    OnPlayerHealthChange?.Invoke(hearts);
  }
  private void OnTriggerStay2D(Collider2D other) {
    if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet")) {
      TakeDamage(CalculateApproximateContactPoint(other).x > 0.5 ? true : false);
      StartCoroutine(TemporaryInvulnerability(gameObject));
    }
    OnPlayerHealthChange?.Invoke(hearts);
  }
  private Vector2 CalculateApproximateContactPoint(Collider2D other) {
    Vector2 ownCenter = GetComponent<Collider2D>().bounds.center;
    Vector2 otherCenter = other.bounds.center;
    Vector2 midPoint = (ownCenter + otherCenter) / 2f;
    return midPoint;
  }
  
  // private void OnCollisionEnter2D(Collision2D collision) {
  //   if (collision.gameObject.CompareTag("Enemy") || 
  //       collision.gameObject.CompareTag("EnemyBullet")) {
  //     TakeDamage(collision.contacts[0].point.x > 0.5 ? true : false);
  //     StartCoroutine(TemporaryInvulnerability(collision.gameObject));
  //   }
  //   OnPlayerHealthChange?.Invoke(hearts);
  // }
  // private void OnCollisionStay2D(Collision2D collision) {
  //   if (collision.gameObject.CompareTag("Enemy") || 
  //       collision.gameObject.CompareTag("EnemyBullet")) {
  //     TakeDamage(collision.contacts[0].point.x > 0.5 ? true : false);
  //     StartCoroutine(TemporaryInvulnerability(collision.gameObject));
  //   }
  //   OnPlayerHealthChange?.Invoke(hearts);
  // }


  private IEnumerator TemporaryInvulnerability(GameObject gameObject) {
    isInvincible = true;

    // foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
    //   obj.GetComponent<Rigidbody2D>().simulated = false;
    // foreach (var obj in GameObject.FindGameObjectsWithTag("EnemyBullet"))
    //   obj.GetComponent<Rigidbody2D>().simulated = false;

    yield return new WaitForSeconds(2);

    // foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
    //   obj.GetComponent<Rigidbody2D>().simulated = true;
    // foreach (var obj in GameObject.FindGameObjectsWithTag("EnemyBullet"))
    //   obj.GetComponent<Rigidbody2D>().simulated = true;

    isInvincible = false;
  }

  /// <summary>
  /// Helper function to take damage. It also changes the Scene state to Lose, so that the scene
  /// object can notify the other Game Objects.
  /// </summary>
  private void TakeDamage(bool isFacingRight) {
    if (isInvincible) return;
    hearts -= 1;
    if (hearts <= 0) {
      FightSceneStateManager.Instance.ChangeState(FightSceneStateManager.SceneState.Lose);
    } else {
      ChangeActionState(new PlayerDamagedState(isFacingRight));
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
        //ChangeMovementState(new PlayerIdleState());
        //ChangeActionState(new PlayerEntryState());
        //ChangeMovementState(new PlayerIntroState());
        
        break;
      case FightSceneStateManager.SceneState.Win:
        ChangeMovementState(new PlayerIdleState());
        ChangeActionState(new PlayerNoneState());
        // Disable the colliders
        break;
      case FightSceneStateManager.SceneState.Lose:
        ChangeActionState(new PlayerDeathState());
        ChangeMovementState(new PlayerDeathMovementState());
        
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

  public void EnterInvincibility() {
    spriteRenderer.material = invincibilityMaterial;
    StartCoroutine(InvincibilityState());
  }
  public void ExitInvincibility() {
    spriteRenderer.material = defaultMaterial;
  }

  private IEnumerator InvincibilityState() {
    isInvincible = true;
    yield return new WaitForSeconds(invincibilityTime);
    isInvincible = false;
    ExitInvincibility();
  }

  public void AddToSuperMeter(float amount) {
    superMeter += amount;
    if (superMeter > 5f) superMeter = 5f;
    OnPlayerSuperMeterChange?.Invoke(superMeter);
  }
  public void RemoveToSuperMeter(float amount) {
    superMeter -= amount;
    if (superMeter < 0f) superMeter = 0f;
    OnPlayerSuperMeterChange?.Invoke(superMeter);
  }

  private void HandleShootPerformed() {
    IsShooting = true;
  }  
  private void HandleShootCanceled() {
    IsShooting = false;
  }  
}
