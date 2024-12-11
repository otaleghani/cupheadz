# How to

``` cs 
// A normal scene change can be triggered with the SceneManager.
using Unity.SceneManagement;

public class SceneChanger : MonoBehavior {
    // You can call a new scene by string
    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    // Or by index
    public void ChangeScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}

// Problem with this approach is the lack of a loading scene. This could be resolved with an asynchronous load.
```

## Asynchronous scenes
``` cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehavior {
    public void ChangeSceneAsync(string sceneName) {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName) {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Prevents the scene from activating immediately
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone) {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            if (asyncOperation.progress >= 0.9f) {
                // Optionally, wait for user input or anther condition
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
```

## Lifetime of a scene

A scene in unity has different phases that you have to handle.
- Loading phase     -> The scene gets loaded in memory
- Active phase      -> Scene is active and so interactable
- Unloading phase   -> Scene is removed from memory

Some data could be made persistent across different scenes, by marking them with DOntDestroyOnLoad.

When working with an async loading you can check the progress with the param `progress`. If you setted the `allowSceneActivation` to true, the scene will activate once it reached 0.9f in the `progress`. If is set to false you can check the progress and do other stuff before activating it, like waiting for a user input, play an audio or whatever.
