using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIMap : MonoBehaviour {
  private TMP_Text bossCardSubtitle;
  private TMP_Text bossCardTitle;

  public void GoInBossfight() {
    // I could like create a way to store the different information about the bosses
    // in one place.
    Debug.Log("Change the scene");
    SceneManager.LoadScene("TestingGrounds");
    Time.timeScale = 1f;
  }
}
