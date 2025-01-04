using UnityEngine;
using UnityEngine.SceneManagement;

public class UiSplashScreen : MonoBehaviour {
  public void Exit() {
    Debug.Log("Quit app");
    Application.Quit();
  }

  public void GoToMap() {
    SceneManager.LoadScene("Map");
  }
}
