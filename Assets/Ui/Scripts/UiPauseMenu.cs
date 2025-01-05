using UnityEngine;
using UnityEngine.SceneManagement;

public class UiPauseMenu : MonoBehaviour {
  public void ReturnToMap() {
    SceneManager.LoadScene("Map");
    Time.timeScale = 1f;
  }

  public void Retry() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    Time.timeScale = 1f;
  }
}
