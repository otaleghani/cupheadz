using System;
using UnityEngine;

public class PlayerGroundCollision : MonoBehaviour {
  public event Action<Collider2D> OnGroundCollisionEnter;
  public event Action<Collider2D> OnGroundCollisionExit;

  private void OnTriggerEnter2D(Collider2D collider) {
    if (collider.CompareTag("Ground")) {
      OnGroundCollisionEnter?.Invoke(collider);
    }
  }
  private void OnTriggerStay2D(Collider2D collider) {
    if (collider.CompareTag("Ground")) {
      OnGroundCollisionEnter?.Invoke(collider);
    }
  }
  private void OnTriggerExit2D(Collider2D collider) {
    if (collider.CompareTag("Ground")) {
      OnGroundCollisionExit?.Invoke(collider);
    }
  }
}
