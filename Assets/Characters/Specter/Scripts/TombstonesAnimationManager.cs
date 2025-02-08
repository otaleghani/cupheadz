using UnityEngine;

public class TombstonesAnimationManager : MonoBehaviour {
 
  private Animator _animator;

  private void Awake() {
    _animator = GetComponent<Animator>();
  }

  public void EndOfAttack() {
    _animator.Play("Idle");
  }
}
