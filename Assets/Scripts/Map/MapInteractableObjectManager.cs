using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class MapInteractableObjectManager : MonoBehaviour {
  public GameObject bossTitleCard;
  private PlayerInputManager inputManager;
  private Button button;

  public virtual void HandleInteract() {
    Time.timeScale = 0f;
    //EventSystem.current.firstSelectedGameObject = bossTitleCard.transform.Find("BackgroundImage/Simple").gameObject;
    //EventSystem.current.SetSelectedGameObject(bossTitleCard.transform.Find("BackgroundImage/Simple").gameObject);
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
    //GameObject button = bossTitleCard.transform.Find("BackgroundImage/Simple").gameObject;
    button.Select();
    bossTitleCard.SetActive(true);
    //EventSystem.current.firstSelectedGameObject = button;
    //EventSystem.current.SetSelectedGameObject(button);
  }
}
