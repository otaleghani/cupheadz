using UnityEngine;

public class IrisTransitionManager : MonoBehaviour {
  public static IrisTransitionManager Instance;
  private Animator _animator;
  private string _irisIn = "IrisTransitionIn";
  private string _irisOut = "IrisTransitionOut";
  private string _none = "IrisTransitionNone";
  

  private void Awake() {
    if (Instance == null) {
      Instance = this; 
    } else {
      Debug.LogWarning("Found more than one ReadyWallupManager in scene. Delete one.");
    }
    _animator = GetComponent<Animator>();
  }

  // This one is called at the ENTER of the fight
  public void PlayIn() {
    _animator.Play(_irisIn);
  }
  public void OnIrisInEnd() {
    _animator.Play(_none);
    ReadyWallupManager.Instance.Play();
  }

  // This one is called
  public void PlayOut() {
    _animator.Play(_irisOut);
  }
  public void OnIrisOutEnd() {
    // TODO: Change scene
  }

  public void PlayIntroPt1() {
    AudioManager.Instance.Play("Soundtrack");
    AudioManager.Instance.Play("IntroPt1");
  }
}
