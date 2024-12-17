using UnityEngine;
using System;

public class SceneStateManager : MonoBehaviour {
  // Singleton instance so that the other script can change the SceneState
  public static SceneStateManager Instance { get; private set; }
  public SceneState currentState { get; private set; }
  public event Action<SceneState> OnChangeState;
  public enum SceneState {
    Entry,
    Exit,
    Play,
    Win,
    Lose
  }

  private void Awake() {
    currentState = SceneState.Entry;
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  public void ChangeState(SceneState newState) {
    currentState = newState;
    // Notifies the new state, so that the other scripts can manage the current gamestate
    OnChangeState?.Invoke(currentState);
  }
}
