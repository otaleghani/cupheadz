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
  // Does it need to have the input manager?
  private GameData.Weapon firstWeaponId;
  private GameData.Weapon secondWeaponId;

  public GameObject firstWeapon;
  public GameObject secondWeapon;

  void Start() {
    firstWeapon = EquipWeapons(firstWeaponId);
    secondWeapon = EquipWeapons(secondWeaponId);
  }

  void Update() {
    //Debug.Log(firstWeapon);
  }

  float counter = 0f;
  float maxCounter = 0.2f;
  void FixedUpdate() {
    if (counter >= maxCounter) {
      Instantiate(firstWeapon, transform.position, Quaternion.identity);
      counter = 0f;
    }
    counter += Time.deltaTime;
  }

  private GameObject EquipWeapons(GameData.Weapon weapon) {
    GameObject weaponObj = null;

    // TODO: Finds the right gameobject for each weapon
    switch (weapon) {
      case GameData.Weapon.Peashooter: 
        weaponObj = Resources.Load<GameObject>("Peashooter__Bullet");
        break;
      default:
        //weaponObj = Resources.Load<GameObject>("Peashooter__Bullet");
        break;
    }
    return weaponObj;
  }

  public void LoadData(GameData gameData) {
    firstWeaponId = gameData.equippedWeapon["first"];
    secondWeaponId = gameData.equippedWeapon["second"];
  }

  public void SaveData(ref GameData gameData) {}
}
