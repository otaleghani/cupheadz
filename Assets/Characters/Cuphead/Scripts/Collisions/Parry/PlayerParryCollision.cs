using System;
using UnityEngine;

public class PlayerParryCollision : MonoBehaviour {
  public event Action<Collider2D> OnParryCollision;
  private new Collider2D collider;

  private void Awake() {
    collider = GetComponent<Collider2D>();
  }

  public void EnableCollider() {
    collider.enabled = true;
  }
  public void DisableCollider() {
    collider.enabled = false;
  }

  private void OnTriggerEnter2D(Collider2D collider) {
    if (collider.CompareTag("Parryable")) {
      OnParryCollision?.Invoke(collider);
    }
  }
  private void OnTriggerStay2D(Collider2D collider) {
    if (collider.CompareTag("Parryable")) {
      OnParryCollision?.Invoke(collider);
    }
  }
}
