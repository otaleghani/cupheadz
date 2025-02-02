using UnityEngine;

public class ReadyWallupManager : MonoBehaviour {
  public static ReadyWallupManager Instance;
  private Animator _animator;
  private string _anim = "ReadyWallupAnimation";
  private string _none = "ReadyWallupNone";

  private void Awake() {
    if (Instance == null) {
      Instance = this; 
    } else {
      Debug.LogWarning("Found more than one ReadyWallupManager in scene. Delete one.");
    }
    _animator = GetComponent<Animator>();
  }

  public void Play() {
    _animator.Play(_anim);
  }
  public void OnAnimationEnd() {
    FightSceneStateManager.Instance.ChangeState(FightSceneStateManager.SceneState.Play);
    _animator.Play(_none);
  }
}
