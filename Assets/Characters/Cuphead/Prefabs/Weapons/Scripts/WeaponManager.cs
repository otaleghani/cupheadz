using UnityEngine;

/// <summary>
/// The weapon manager has to
/// - It holds the data related to the equipped weapons
/// - Initializes 2 pools of bullets for each weapon
/// - Spawn on bullet at a time
/// - Play the flash animation of the current bullet
/// - And finally it will keep track of the time passed from each bullet
/// </summary>
public class WeaponManager : MonoBehaviour, IDataPersistence {
  private WeaponAnimatorManager animatorManager;
  private PlayerStateManager stateManager;
  private PlayerMovementManager movementManager;
  
  // Todo: handle weapons change, remember to hold the counter of both bullets
  private PlayerInputManager inputManager;  


  public GameData.Weapon firstWeaponId;
  public GameData.Weapon secondWeaponId;
  private GameObject firstWeapon;
  private GameObject secondWeapon;
  private float firstWeaponFireRate;
  private float secondWeaponFireRate;

  private GameObject equippedWeapon;
  private float equippedWeaponFireRate;
  private float shootCounter;

  // Todo: handle weapon aim position
  private int xDirection;
  private int yDirection;

  void Start() {
    stateManager = GetComponentInParent<PlayerStateManager>();
    movementManager = GetComponentInParent<PlayerMovementManager>();
    animatorManager = GetComponent<WeaponAnimatorManager>();
    (firstWeapon, firstWeaponFireRate) = EquipWeapons(firstWeaponId);
    (secondWeapon, secondWeaponFireRate) = EquipWeapons(secondWeaponId);
    equippedWeapon = firstWeapon;
    equippedWeaponFireRate = firstWeaponFireRate;
  }

  void FixedUpdate() {
    if (shootCounter <= equippedWeaponFireRate) { 
      shootCounter += Time.deltaTime;
      return;
    }

    if (stateManager.actionState is PlayerShootingState) {
      // Todo: Play "sparkle" animation based on the equipped weapon
      animatorManager.SetParameterIsShooting();

      if (stateManager.movementState is PlayerAimState) {
        // Todo: Handle bullet direction based on the current aim direction
        return;
      }
      Shoot();
    }
  }

  private void Shoot() {
    GameObject bulletIns = Instantiate(firstWeapon, transform.position, Quaternion.identity);
    Bullet bullet = bulletIns.GetComponent<Bullet>();
    if (!movementManager.isFacingRight) {
      bullet.direction = Vector2.left;
      bullet.FlipBullet();
    }

    shootCounter = 0f;
  }

  /// <summary>
  /// Based on the enum provided, it returns the right GameObject and fireRate
  /// </summary>
  private (GameObject, float) EquipWeapons(GameData.Weapon weapon) {
    GameObject weaponObj = null;
    float fireRate = 0f;

    switch (weapon) {
      case GameData.Weapon.Peashooter: 
        weaponObj = Resources.Load<GameObject>("Peashooter__Bullet");
        fireRate = 0.2f;
        break;

      default:
        weaponObj = Resources.Load<GameObject>("Peashooter__Bullet");
        fireRate = 0.2f;
        break;
    }
    return (weaponObj, fireRate);
  }

  public void LoadData(GameData gameData) {
    firstWeaponId = gameData.equippedWeapon["first"];
    secondWeaponId = gameData.equippedWeapon["second"];
  }

  public void SaveData(ref GameData gameData) {}
}
