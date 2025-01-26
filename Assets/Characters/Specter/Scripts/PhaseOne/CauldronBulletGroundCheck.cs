using UnityEngine;
using System;

public class CauldronBulletGroundCheck : MonoBehaviour {
  public event Action GroundDetected;
  
  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Ground")) {
      GroundDetected?.Invoke();
    }
  }
  private void OnTriggerStay2D(Collider2D other) {
    if (other.CompareTag("Ground")) {
      GroundDetected?.Invoke();
    }
  }
}
