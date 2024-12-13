using UnityEngine;

/// Class used to play the sparkle animation.
public abstract class BulletSparkle: MonoBehaviour {

  public virtual void OnSparkleAnimationEnd() {
    Destroy(gameObject);
  }
}
