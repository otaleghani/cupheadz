using UnityEngine;

/// Class that spawns the "big-ass" mug, while cuphead is transforming in the invincibility form
/// Only thing that this does is playing the animation and then destroying itself
public class InvincibilityManager : MonoBehaviour {
  public void OnAnimationEnd() {
    Destroy(gameObject);
  }
}
