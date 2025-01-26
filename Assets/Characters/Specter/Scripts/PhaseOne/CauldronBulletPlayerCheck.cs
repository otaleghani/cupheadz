using UnityEngine;
using System;

public class CauldronBulletPlayerCheck : MonoBehaviour {
  public event Action PlayerDetected;
  
  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      PlayerDetected?.Invoke();
    }
  }
  private void OnTriggerStay2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      PlayerDetected?.Invoke();
    }
  }
}
