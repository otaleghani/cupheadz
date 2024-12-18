using UnityEngine;

/// Class used to play the sparkle animation.
public abstract class BulletSparkle: MonoBehaviour {
  private WeaponManager weaponManager;

  protected virtual void Awake() {
    weaponManager = FindFirstObjectByType<WeaponManager>();
  }

  public virtual void OnSparkleAnimationEnd() {
    weaponManager.ReturnSparkle(gameObject);
  }
}
