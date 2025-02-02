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
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  private void Start() {
    ChangeState(SceneState.Entry);
  }

  private void FixedUpdate() {
    Debug.Log(currentState);
  }

  public void ChangeState(SceneState newState) {
    currentState = newState;
    // Notifies the new state, so that the other scripts can manage the current gamestate
    OnChangeState?.Invoke(currentState);
    HandleNewState();
  }

  public void HandleNewState() {
    switch (currentState) {
      case SceneState.Entry:
        IrisTransitionManager.Instance.PlayIn();
        // Play animation of cuphead
        // Play animation of boss
        //BossStateManager.Instance.
        break;
    }
  }
}
