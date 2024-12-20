using UnityEngine;

/// <summary>
/// Manages the Sparkle prefab, so the creation and the AnimationEnd
/// </summary>
public abstract class ExBulletSparkle: MonoBehaviour {
  private WeaponManager weaponManager;

  protected virtual void Awake() {
    weaponManager = FindFirstObjectByType<WeaponManager>();
  }

  /// <summary>
  /// This method runs whenever the ExplosionAnimation of the bullet ends.
  /// </summary>
  public virtual void OnSparkleAnimationEnd() {
    weaponManager.ReturnSparkle(gameObject);
  }
}
