//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour {
  [Header("File Storage Config")]
  [SerializeField] private string fileName;
  [SerializeField] private bool useEncryption;

  private GameData gameData;
  private List<IDataPersistence> dataPersistenceObjects;
  private FileDataHandler dataHandler;
  public static DataPersistenceManager instance { get; private set; }

  private void Awake() {
    if (instance != null) {
      Debug.Log("Found more than one DataPersistenceManager in the scene.");
    }
    instance = this;
  }

  private void Start() {
    this.dataHandler = new FileDataHandler(
      Application.persistentDataPath, 
      fileName,
      useEncryption
    );
    this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    LoadGame();
  }

  public void NewGame() {
    this.gameData = new GameData();
  }

  public void LoadGame() {
    this.gameData = dataHandler.Load();
    if (this.gameData == null) {
      Debug.Log("No GameData found. Initializing data to defaults.");
      NewGame();
    }
    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
      dataPersistenceObj.LoadData(gameData);
    }
    Debug.Log("Loaded. Count = " + gameData.deathCount);
  }

  public void SaveGame() {
    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
      dataPersistenceObj.SaveData(ref gameData);
    }
    Debug.Log("Saved. Count = " + gameData.deathCount);
    dataHandler.Save(gameData);
  }

  private void OnApplicationQuit() {
    SaveGame();
  }

  private List<IDataPersistence> FindAllDataPersistenceObjects() {
    IEnumerable<IDataPersistence> dataPersistenceObjects =
      FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();
    return new List<IDataPersistence>(dataPersistenceObjects);
  }
}
