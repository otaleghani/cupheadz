using UnityEngine;
using System;

/// <summary>
/// Scene manager is used in every scene where there is gameplay, so run and gun and 
/// bossfights scenes.
/// </summary>
public class FightSceneStateManager : MonoBehaviour {
  // Singleton instance so that the other script can change the SceneState
  public static FightSceneStateManager Instance { get; private set; }
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
      //DontDestroyOnLoad(gameObject);
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
