using UnityEngine;
public class ParrySpark : MonoBehaviour {
  private Animator animator;
  private string animationToPlay;
  private ParrySparksManager manager;

  //private poolManager

  private void Awake() {
    animator = GetComponent<Animator>();
    animationToPlay = "ParrySpark__Hand";
    manager = GameObject.Find("ParrySparksManager").GetComponent<ParrySparksManager>();
  }

  private void Start() {
    if (CupheadCharmsManager.Instance.equippedCharm[GameData.Charm.Wheatstone]) {
      animationToPlay = "ParrySpark__Axe";
    }
  }

  private void OnEnable() {
    animator.Play(animationToPlay);
  }

  public void OnParrySparkAnimationEnd() {
    manager.ReturnSpark(gameObject);
  }
}
