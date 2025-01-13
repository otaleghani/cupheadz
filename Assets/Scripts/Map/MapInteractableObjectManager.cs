using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.EventSystems;

public class MapInteractableObjectManager : MonoBehaviour, IDataPersistence {
  //public GameData.Boss boss;
  public enum Tester {
    Energy,
    Invinci,
    Ghost,
    SpecterIdle
  }
  public Tester test;
  public GameObject bossTitleCard;
  private PlayerInputManager inputManager;
  private Button button;

  private GameData.Super equipped;

  public virtual void HandleInteract() {
    if (test == Tester.Energy) {
      bossTitleCard.transform.Find("BackgroundImage/Subtitle").GetComponent<TMP_Text>().text = "Super Raggione Galattico";
      bossTitleCard.transform.Find("BackgroundImage/Title").GetComponent<TMP_Text>().text = "anvendi nando ";
      //equipped = GameData.Super.EnergyBeam;
      //DataPersistenceManager.instance.SaveGame();
      DataPersistenceManager.instance.ChangeSuper(GameData.Super.EnergyBeam);
      Debug.Log("Im in energy");
    }
    if (test == Tester.Invinci) {
      bossTitleCard.transform.Find("BackgroundImage/Subtitle").GetComponent<TMP_Text>().text = "Super Invicibilitaz";
      bossTitleCard.transform.Find("BackgroundImage/Title").GetComponent<TMP_Text>().text = "sgravo fiscale";
      //equipped = GameData.Super.Invincibility;
      //DataPersistenceManager.instance.SaveGame();
      DataPersistenceManager.instance.ChangeSuper(GameData.Super.Invincibility);
      Debug.Log("Im in invinci");
    }
    if (test == Tester.Ghost) {
      bossTitleCard.transform.Find("BackgroundImage/Subtitle").GetComponent<TMP_Text>().text = "Super sonico";
      bossTitleCard.transform.Find("BackgroundImage/Title").GetComponent<TMP_Text>().text = "Sgrodolo supremo";
      //equipped = GameData.Super.GiantGhost;
      //DataPersistenceManager.instance.SaveGame();
      DataPersistenceManager.instance.ChangeSuper(GameData.Super.GiantGhost);
      Debug.Log("Im in energy");
    }

    //switch (test) {
    //  case Tester.Energy: 
    //    break;
    //  case Tester.Invinci: 
    //    break;
    //  case Tester.Ghost: 
    //    break;
    //  case Tester.SpecterIdle: 
    //    bossTitleCard.transform.Find("BackgroundImage/Subtitle").GetComponent<TMP_Text>().text = "Specterone";
    //    bossTitleCard.transform.Find("BackgroundImage/Title").GetComponent<TMP_Text>().text = "Anvedi smooth";
    //    break;
    //}
    Time.timeScale = 0f;
    button = bossTitleCard.transform.Find("BackgroundImage/Simple").GetComponent<Button>();
    inputManager = GameObject.Find("CupheadOverworld").GetComponent<PlayerInputManager>();
    inputManager.SwitchToUi();
    inputManager.OnUICancel += HandleCancel;
    StartCoroutine(SetSelectedButtonNextFrame());
  }

  private void HandleCancel() {
    Time.timeScale = 1f;
    bossTitleCard.SetActive(false);
    inputManager.SwitchToPlayer();
    inputManager.OnUICancel -= HandleCancel;
  }

  private IEnumerator SetSelectedButtonNextFrame() {
    yield return null; // Wait for the next frame
    button.Select();
    bossTitleCard.SetActive(true);
  }

  public void SaveData(ref GameData data) {
    data.equippedSuper = equipped;
    Debug.Log(equipped);
  }
  public void LoadData(GameData data) {
    //equipped = data.equippedSuper;
  }
}
