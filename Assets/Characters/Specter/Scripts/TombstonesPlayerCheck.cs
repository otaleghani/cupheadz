using UnityEngine;

public class TombstonesPlayerCheck : MonoBehaviour {

  private Animator _animator;

  private void Awake() {
    _animator = GetComponentInParent<Animator>();
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      _animator.Play("Attack");
    }
  }
}
