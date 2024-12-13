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
  public enum ShootDirection {
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
  }
  public Dictionary<string, ShootDirection> coordinates = 
    new Dictionary<string, ShootDirection>();
  public Dictionary<ShootDirection, Transform> firePoints = 
    new Dictionary<ShootDirection, Transform>();

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

  private int xDirection = 1;
  private int yDirection = 0;

  void Awake() {
    stateManager = GetComponentInParent<PlayerStateManager>();
    movementManager = GetComponentInParent<PlayerMovementManager>();
    inputManager = GetComponentInParent<PlayerInputManager>();

    firePoints[ShootDirection.Up] = transform.Find("Up");
    firePoints[ShootDirection.Down] = transform.Find("Down");
    firePoints[ShootDirection.Left] = transform.Find("Left");
    firePoints[ShootDirection.Right] = transform.Find("Right");
    firePoints[ShootDirection.UpLeft] = transform.Find("UpLeft");
    firePoints[ShootDirection.UpRight] = transform.Find("UpRight");
    firePoints[ShootDirection.DownLeft] = transform.Find("DownLeft");
    firePoints[ShootDirection.DownRight] = transform.Find("DownRight");

    // Todo: delete right side things
    coordinates["0,1"] = ShootDirection.Up;
    coordinates["0,-1"] = ShootDirection.Down;
    coordinates["1,0"] = ShootDirection.Left;
    coordinates["-1,0"] = ShootDirection.Right;
    coordinates["1,1"] = ShootDirection.UpLeft;
    coordinates["-1,1"] = ShootDirection.UpRight;
    coordinates["1,-1"] = ShootDirection.DownLeft;
    coordinates["-1,-1"] = ShootDirection.DownRight;
  }

  void Start() {
    firstWeaponObj = EquipWeaponsObj(firstWeaponId);
    secondWeaponObj = EquipWeaponsObj(secondWeaponId);
    firstWeapon = firstWeaponObj.GetComponent<WeaponManager>();
    secondWeapon = secondWeaponObj.GetComponent<WeaponManager>();
    equippedWeapon = firstWeapon;
  }

  void OnEnable() {
    inputManager.OnMovePerformed += HandleOnMovePerformed;
    inputManager.OnMoveCanceled += HandleOnMoveCanceled;
  }
  void OnDisable() {
    inputManager.OnMovePerformed -= HandleOnMovePerformed;
    inputManager.OnMoveCanceled -= HandleOnMoveCanceled;
  }

  void FixedUpdate() {
    if (shootCounter <= equippedWeapon.fireRate) { 
      shootCounter += Time.deltaTime;
      return;
    } 

    if (stateManager.actionState is PlayerShootingState) {
      if (stateManager.movementState is PlayerAimState) {
        if (xDirection == 0 && yDirection == 0) {
          xDirection = movementManager.isFacingRight ? 1 : -1;
        }
        equippedWeapon.Shoot(xDirection, yDirection, GetSpawn(xDirection, yDirection));
      } else {
        int xPos = movementManager.isFacingRight ? 1 : -1;
        equippedWeapon.Shoot(xPos, 0, GetSpawn(xPos, 0));
      }
      shootCounter = 0f;
    }
  }

  private Transform GetSpawn(int x, int y) {
    return firePoints[coordinates[x + "," + y]];
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

  private void HandleOnMovePerformed(Vector2 vector) {
    xDirection = Mathf.RoundToInt(vector.x);
    yDirection = Mathf.RoundToInt(vector.y);
  }
  private void HandleOnMoveCanceled() {
    xDirection = 0;
    yDirection = 0;
  }
}
