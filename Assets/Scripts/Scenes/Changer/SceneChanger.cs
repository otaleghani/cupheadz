using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
  public static SceneChanger Instance;
  public enum Scene {
    SplashScreen,
    Intro,
    Map,
    TestingGrounds,
    BossSpecter,
  }
  private Dictionary<Scene, string> scenes = new Dictionary<Scene, string>();

  public void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }

    scenes[Scene.SplashScreen] = "Splash";
    scenes[Scene.Intro] = "Intro";
    scenes[Scene.Map] = "Map";
    scenes[Scene.TestingGrounds] = "TestingGrounds";
  }


  public void ChangeScene(Scene newScene) {
    SceneManager.LoadScene(scenes[newScene]);
  }

  public void ChangeSceneAsync(Scene newScene) {
    LoadSceneAsyncCoroutine(newScene);
  }


  private IEnumerator<int> LoadSceneAsyncCoroutine(Scene newScene) {
    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scenes[newScene]);
    // Stops auto actiovation
    asyncOperation.allowSceneActivation = false; 

    while (!asyncOperation.isDone) {
      float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
      if (asyncOperation.progress >= 0.9f) {
        // Right now it just waits until the progress is 90% (which means is done)
        asyncOperation.allowSceneActivation = true;
      }
      yield return 1;
    }
  }
}
