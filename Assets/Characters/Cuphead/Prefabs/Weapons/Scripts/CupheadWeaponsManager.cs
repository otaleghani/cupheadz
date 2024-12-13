using UnityEngine;

/// <summary>
/// The weapon manager has to
/// - It holds the data related to the equipped weapons
/// - Initializes 2 pools of bullets for each weapon
/// - Spawn on bullet at a time
/// - Play the flash animation of the current bullet
/// - And finally it will keep track of the time passed from each bullet
/// </summary>
public class CupheadWeaponManager : MonoBehaviour, IDataPersistence {
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
  }
  void OnDisable() {
    inputManager.OnMovePerformed -= HandleOnMovePerformed;
  }

  void FixedUpdate() {
    if (shootCounter <= equippedWeapon.fireRate) { 
      shootCounter += Time.deltaTime;
      return;
    } else if (stateManager.actionState is PlayerShootingState) {

      if (stateManager.movementState is PlayerAimState) {
        equippedWeapon.Shoot(xDirection, yDirection);
      } else {
        equippedWeapon.Shoot(xDirection, 0);
      }
      shootCounter = 0f;
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

  private void HandleOnMovePerformed(Vector2 vector) {
    xDirection = Mathf.RoundToInt(vector.x);
    yDirection = Mathf.RoundToInt(vector.y);
  }
}
