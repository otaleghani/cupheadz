using UnityEngine;
using System;

/// <summary>
/// Scene manager is used in every scene where there is gameplay, so run and gun and 
/// bossfights scenes.
/// </summary>
[DefaultExecutionOrder(-900)]
public class FightSceneStateManager : MonoBehaviour {
  // Singleton instance so that the other script can change the SceneState
  public static FightSceneStateManager Instance { get; private set; }

  public enum SceneName {
    Specter,
  }
  
  public SceneName CurrentScene;
  
  public SceneState currentState { get; private set; }
  
  public event Action<SceneState> OnChangeState;
  public enum SceneState {
    Entry,
    Exit,
    Play,
    Win,
    Lose
  }

  public GameObject DeathCard;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
    DeathCard = Instantiate(DeathCard, transform.Find("Canvas").transform);
  }

  private void Start() {
    ChangeState(SceneState.Entry);
    DeathCard.SetActive(false);
  }

  private void FixedUpdate() {
    // Debug.Log(currentState);
  }

  public void ChangeState(SceneState newState) {
    currentState = newState;
    // Notifies the new state, so that the other scripts can manage the current gamestate
    OnChangeState?.Invoke(currentState);
    HandleNewState();
  }

  public void HandleNewState() {
    Debug.Log("Called HandleNewState");
    switch (currentState) {
      case SceneState.Entry:
        IrisTransitionManager.Instance.PlayIn();
        // Play animation of cuphead
        // Play animation of boss
        //BossStateManager.Instance.
        break;
      case SceneState.Lose:
        transform.Find("Canvas/YouDied").gameObject.SetActive(true);
        break;
    }
  }

  public void ActivateDeathCard(string name) {
    DeathCard.SetActive(true);
    DeathCardManager.Instance.ToDisplay();
    BossStateManager.Instance.DeathCard();
    PlayerStateManager.instance.inputManager.SwitchToUi();
  }
}
