using UnityEngine;
using System.Collections.Generic;

public class CupheadCharmsManager : MonoBehaviour, IDataPersistence {
  public static CupheadCharmsManager Instance;

  public Dictionary<GameData.Charm, bool> equippedCharm = new Dictionary<GameData.Charm, bool>();

  private void Awake() {
    if (Instance == null) Instance = this;
  }

  public void LoadData(GameData gameData) {
    equippedCharm = gameData.equippedCharm;
  }
  public void SaveData(ref GameData gameData) {}
}
