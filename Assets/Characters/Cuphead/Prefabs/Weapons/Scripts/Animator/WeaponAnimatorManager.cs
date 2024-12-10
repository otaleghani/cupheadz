using UnityEngine;
public class WeaponAnimatorManager : MonoBehaviour {
  private Animator animator;

  private int AnimatorIsShooting;

  void Start() {
    animator = GetComponent<Animator>();
    AnimatorIsShooting = Animator.StringToHash("IsShooting");
  }

  public void ResetParameters() {
    animator.SetBool(AnimatorIsShooting, false);
  }
  public void SetParameterIsShooting() {
    animator.SetBool(AnimatorIsShooting, true);
  }

  public void OnSparkleEnd() {
    ResetParameters();
  }
}
