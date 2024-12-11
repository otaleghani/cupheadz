Creating the UI means that you'll have to think about different things:
- halting the current scene,
- displaying the correct menu,
- managing the UI element for navigation and interaction.

## How to Halt the main scene

``` cs
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public static bool IsPaused = false;

    public GameObject pauseMenuUI;

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused) 
                Resume();
            else 
                Pause();
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        AudioListener.pause = false;
    }

    // Implement different UI logic...
    public void QuitGame() {
        Debug.Log("Quitting game...);
    }
}
```
