using UnityEngine;

/// <summary>
/// Instanciate the correct equipped super
/// </summary>
public class CupheadSuperManager : MonoBehaviour, IDataPersistence {
  public GameData.Super equippedSuper;
  private ISuperAttack superAttack;
  private GameObject superObj;
  public PlayerMovementManager movementManager;
  public PlayerStateManager stateManager;

  private void Awake() {}

  private void Start() {
    movementManager = GetComponentInParent<PlayerMovementManager>();
    stateManager = GetComponentInParent<PlayerStateManager>();
    superObj = EquipSuper(equippedSuper);
    superAttack = superObj.GetComponent<ISuperAttack>();
  }

  public void UseSuper() {
    superAttack.UseSuper();
  }


  private GameObject EquipSuper(GameData.Super super) {
    GameObject obj = null;
    switch (super) {
      case GameData.Super.EnergyBeam: 
        obj = Instantiate(Resources.Load<GameObject>("EnergyBeam__Super"), transform);
        obj.name = "EnergyBeam";
        break;
      case GameData.Super.Invincibility: 
        obj = Instantiate(Resources.Load<GameObject>("Invincibility__Super"), transform);
        obj.name = "Invincibility";
        break;
      case GameData.Super.GiantGhost: 
        obj = Instantiate(Resources.Load<GameObject>("GiantGhost__Super"), transform);
        obj.name = "GiantGhost";
        break;
    }
    return obj;
  }

  public void LoadData(GameData gameData) {
    equippedSuper = gameData.equippedSuper;
  }
  public void SaveData(ref GameData gameData) {}
}
