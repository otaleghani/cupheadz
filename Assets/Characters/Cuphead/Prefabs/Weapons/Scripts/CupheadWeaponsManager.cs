using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Instantiates current equipped weapons and calls either the Shoot() or ShootEx() method from
/// the weapon with the correct bullet coordinates (used for the direction and rotation of the
/// bullet) and correct spawn position (used for aiming and running state).
///
/// The firePoints are connected with the enum PlayerInputManager.AimDirection, so that we can
/// simply search the right key in the dictionary to assign the correct Transform.
/// </summary>
public class CupheadWeaponManager : MonoBehaviour, IDataPersistence {
  private Dictionary<PlayerInputManager.AimDirection, Transform> firePoints = 
    new Dictionary<PlayerInputManager.AimDirection, Transform>();
  private Dictionary<PlayerInputManager.AimDirection, Transform> fireExPoints = 
    new Dictionary<PlayerInputManager.AimDirection, Transform>();
  private Transform movingFirePoint; 
  private Transform crouchingFirePoint; 

  private PlayerStateManager stateManager;
  private PlayerMovementManager movementManager;
  private PlayerInputManager inputManager;
  private PlayerAnimatorManager animatorManager;

  public GameData.Weapon firstWeaponId;
  public GameData.Weapon secondWeaponId;
  private WeaponManager firstWeapon;
  private WeaponManager secondWeapon;
  private WeaponManager equippedWeapon;
  private GameObject firstWeaponObj;
  private GameObject secondWeaponObj;

  private float firstWeaponFireRate;
  private float secondWeaponFireRate;
  private float shootCounter;
  private float exShootCounter;

  private int xDirection;
  private int yDirection;

  private void Awake() {
    stateManager = GetComponentInParent<PlayerStateManager>();
    movementManager = GetComponentInParent<PlayerMovementManager>();
    inputManager = GetComponentInParent<PlayerInputManager>();
    animatorManager = GetComponentInParent<PlayerAnimatorManager>();

    firePoints[PlayerInputManager.AimDirection.Up] = transform.Find("Up");
    firePoints[PlayerInputManager.AimDirection.Down] = transform.Find("Down");
    firePoints[PlayerInputManager.AimDirection.Front] = transform.Find("Front");
    firePoints[PlayerInputManager.AimDirection.DiagonalUp] = transform.Find("DiagonalUp");
    firePoints[PlayerInputManager.AimDirection.DiagonalDown] = transform.Find("DiagonalDown");

    fireExPoints[PlayerInputManager.AimDirection.Up] = transform.Find("UpEx");
    fireExPoints[PlayerInputManager.AimDirection.Down] = transform.Find("DownEx");
    fireExPoints[PlayerInputManager.AimDirection.Front] = transform.Find("FrontEx");
    fireExPoints[PlayerInputManager.AimDirection.DiagonalUp] = transform.Find("DiagonalUpEx");
    fireExPoints[PlayerInputManager.AimDirection.DiagonalDown] = transform.Find("DiagonalDownEx");

    movingFirePoint = transform.Find("Moving");
    crouchingFirePoint = transform.Find("Crouching");
  }

  private void OnEnable() {
    animatorManager.OnExShootingAnimationMidPoint += HandleShootEx;
  }
  private void OnDisable() {
    animatorManager.OnExShootingAnimationMidPoint -= HandleShootEx;
  }


  void Start() {
    firstWeaponObj = EquipWeaponsObj(firstWeaponId);
    secondWeaponObj = EquipWeaponsObj(secondWeaponId);
    firstWeapon = firstWeaponObj.GetComponent<WeaponManager>();
    secondWeapon = secondWeaponObj.GetComponent<WeaponManager>();
    equippedWeapon = firstWeapon;
  }

  void FixedUpdate() {
    exShootCounter += Time.deltaTime;
    shootCounter += Time.deltaTime;
    HandleShoot();
    //HandleShootEx();
  }

  /// <summary>
  /// Calculates the current direction
  /// </summary>
  private void GetDirections() {
    xDirection = inputManager.xPosition;
    yDirection = inputManager.yPosition;
  }
  private void CalculateDirection() {
    if (xDirection == 0) xDirection = movementManager.isFacingRight ? 1 : -1;
  }
  private void CalculateDirectionOnJump() {
    if (yDirection == 1) xDirection = 0;
  }
  private void CalculateDirectionOnAim() {
    if (xDirection == 0 && yDirection == 0) xDirection = movementManager.isFacingRight ? 1 : -1;
  }

  /// <summary>
  /// Calls the weapon ShootEx method with the right directions and spawn point
  /// </summary>
  private void HandleShootEx() {
    GetDirections();
    if (xDirection == 0 && yDirection == 0) {
      movementManager.ExRecoil();
    }
    CalculateDirectionOnAim();
    equippedWeapon.ExShoot(xDirection, yDirection, 
      fireExPoints[PlayerInputManager.CurrentCoordinate],
      fireExPoints[PlayerInputManager.CurrentOppositeCoordinate]);
  }

  /// <summary>
  /// Calls the weapon Shoot method with the right directions and spawn point
  /// </summary>
  private void HandleShoot() {
    if (stateManager.actionState is PlayerShootingState) {
      if (shootCounter <= equippedWeapon.fireRate) return;
      GetDirections();
      switch (stateManager.movementState) {
        case PlayerMovingState: 
          CalculateDirection();
          equippedWeapon.Shoot(xDirection, yDirection, 
            movingFirePoint);
          break;

        case PlayerAimState: 
          CalculateDirectionOnAim();
          equippedWeapon.Shoot(xDirection, yDirection, 
            firePoints[PlayerInputManager.CurrentCoordinate]);
          break;

        case PlayerJumpingState:
          CalculateDirectionOnJump();
          equippedWeapon.Shoot(xDirection, yDirection, 
            firePoints[PlayerInputManager.AimDirection.Front]);
          break;

        case PlayerCrouchState:
          CalculateDirection();
          equippedWeapon.Shoot(xDirection, 0, 
            crouchingFirePoint);
          break;

        case PlayerIdleState:
          //CalculateDirection();
          //equippedWeapon.Shoot(xDirection, 0, 
          //  firePoints[PlayerInputManager.AimDirection.Front]);
          CalculateDirectionOnAim();
          equippedWeapon.Shoot(xDirection, yDirection, 
            firePoints[PlayerInputManager.CurrentCoordinate]);
          break;
      }
      stateManager.currentShootingState = PlayerStateManager.ShootingState.Recoil;
      shootCounter = 0f;
    }
  }

  /// <summary>
  /// Based on current weapon id, Instantiate correct weapon and return the instance
  /// </summary>
  private GameObject EquipWeaponsObj(GameData.Weapon weapon) {
    GameObject obj = null;
    switch (weapon) {
      case GameData.Weapon.Peashooter: 
        obj = Instantiate(Resources.Load<GameObject>("Peashooter__Weapon"), transform);
        obj.name = "Peashooter__Weapon";
        break;

      case GameData.Weapon.Chase:
        obj = Instantiate(Resources.Load<GameObject>("Chaser__Weapon"), transform);
        obj.name = "Chaser__Weapon";
        break;

      case GameData.Weapon.None:
        obj = Instantiate(Resources.Load<GameObject>("Peashooter__Weapon"), transform);
        obj.name = "Chaser__Weapon";
        break;

      default:
        break;
    }
    return obj;
  }
  
  /// <summary>
  /// Loads current equipped weapon id
  /// </summary>
  public void LoadData(GameData gameData) {
    firstWeaponId = gameData.equippedWeapon["first"];
    secondWeaponId = gameData.equippedWeapon["second"];
  }
  public void SaveData(ref GameData gameData) {}
}
