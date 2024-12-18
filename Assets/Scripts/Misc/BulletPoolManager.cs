using UnityEngine;

public class BulletPoolManager : MonoBehaviour {
  public static BulletPoolManager Instance { get; private set; }

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    }
  }
}
