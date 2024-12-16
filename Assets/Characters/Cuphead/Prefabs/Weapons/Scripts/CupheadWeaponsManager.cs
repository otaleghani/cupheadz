using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The weapon manager has to
/// - It holds the data related to the equipped weapons
/// - Initializes 2 pools of bullets for each weapon
/// - Spawn on bullet at a time
/// - Play the flash animation of the current bullet
/// - And finally it will keep track of the time passed from each bullet
/// </summary>
public class CupheadWeaponManager : MonoBehaviour, IDataPersistence {
  private Dictionary<PlayerInputManager.AimDirection, Transform> firePoints = 
    new Dictionary<PlayerInputManager.AimDirection, Transform>();
  private Transform movingFirePoint; 

  private PlayerStateManager stateManager;
  private PlayerMovementManager movementManager;
  private PlayerInputManager inputManager;

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

  // I could've got this from PlayerInputManager?
  private int xDirection = 1;
  private int yDirection = 0;

  void Awake() {
    stateManager = GetComponentInParent<PlayerStateManager>();
    movementManager = GetComponentInParent<PlayerMovementManager>();
    inputManager = GetComponentInParent<PlayerInputManager>();

    firePoints[PlayerInputManager.AimDirection.Up] = transform.Find("Up");
    firePoints[PlayerInputManager.AimDirection.Down] = transform.Find("Down");
    firePoints[PlayerInputManager.AimDirection.Front] = transform.Find("Front");
    firePoints[PlayerInputManager.AimDirection.DiagonalUp] = transform.Find("DiagonalUp");
    firePoints[PlayerInputManager.AimDirection.DiagonalDown] = transform.Find("DiagonalDown");
    movingFirePoint = transform.Find("Moving");
  }

  void OnEnable() {
    inputManager.OnSerializedMovePerformed += HandleAimPosition;
    inputManager.OnSerializedMoveCanceled += HandleAimPosition;
  }
  void OnDisable() {
    inputManager.OnSerializedMovePerformed -= HandleAimPosition;
    inputManager.OnSerializedMoveCanceled -= HandleAimPosition;
  }

  void Start() {
    firstWeaponObj = EquipWeaponsObj(firstWeaponId);
    secondWeaponObj = EquipWeaponsObj(secondWeaponId);
    firstWeapon = firstWeaponObj.GetComponent<WeaponManager>();
    secondWeapon = secondWeaponObj.GetComponent<WeaponManager>();
    equippedWeapon = firstWeapon;
  }

  void FixedUpdate() {
    if (shootCounter <= equippedWeapon.fireRate) { 
      shootCounter += Time.deltaTime;
      stateManager.currentShootingState = PlayerStateManager.ShootingState.Aim;
      return;
    } 

    if (stateManager.actionState is PlayerShootingState) {
      if (stateManager.movementState is PlayerMovingState) {
        xDirection = movementManager.isFacingRight ? 1 : -1;
        equippedWeapon.Shoot(xDirection, 0, movingFirePoint);
      }
      if (stateManager.movementState is PlayerAimState) {
        if (xDirection == 0 && yDirection == 0) {
          xDirection = movementManager.isFacingRight ? 1 : -1;
        }
        equippedWeapon.Shoot(xDirection, yDirection, firePoints[PlayerInputManager.CurrentCoordinate]);
      } 
      if (stateManager.movementState is PlayerIdleState) {
        xDirection = movementManager.isFacingRight ? 1 : -1;
        equippedWeapon.Shoot(xDirection, 0, firePoints[PlayerInputManager.AimDirection.Front]);
      }

      shootCounter = 0f;
      // Here you should change the shooting state
      stateManager.currentShootingState = PlayerStateManager.ShootingState.Recoil;
    }
  }

  /// <summary>
  /// Based on the enum provided, it returns the right weapon game object
  /// </summary>
  private GameObject EquipWeaponsObj(GameData.Weapon weapon) {
    GameObject obj = null;
    switch (weapon) {
      case GameData.Weapon.Peashooter: 
        obj = Instantiate(Resources.Load<GameObject>("Peashooter__Weapon"), transform);
        break;

      // Todo: Add other weapons

      default:
        break;
    }
    return obj;
  }
  
  public void LoadData(GameData gameData) {
    firstWeaponId = gameData.equippedWeapon["first"];
    secondWeaponId = gameData.equippedWeapon["second"];
  }

  public void SaveData(ref GameData gameData) {}

  public void HandleAimPosition(int x, int y) {
    xDirection = x;
    yDirection = y;
  }
}
