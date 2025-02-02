using UnityEngine;

// I think that I can trash this one because the animations are related to other prefabs, not this one
public class FightSceneAnimatorManager : MonoBehaviour {
  private Animator animator;

  private void OnEnable() {
    FightSceneStateManager.Instance.OnChangeState += HandleSceneStateChange;
  }
  private void Awake() {
    animator = GetComponent<Animator>();
  }

  private void HandleSceneStateChange(FightSceneStateManager.SceneState currenteState) {
    switch (currenteState) {
      case FightSceneStateManager.SceneState.Entry: 
        break;
      case FightSceneStateManager.SceneState.Exit: 
        break;
      case FightSceneStateManager.SceneState.Win: 
        break;
      case FightSceneStateManager.SceneState.Lose: 
        break;
      case FightSceneStateManager.SceneState.Play: 
        break;
      default:
        break;
    }
  }

  public void OnSceneAnimationExitEnd() {
    SceneChanger.Instance.ChangeScene(SceneChanger.Scene.Map);
  }
}
