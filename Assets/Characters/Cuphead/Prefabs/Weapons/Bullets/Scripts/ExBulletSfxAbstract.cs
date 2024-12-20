using UnityEngine;

/// <summary>
/// Manages the Sparkle prefab, so the creation and the AnimationEnd
/// </summary>
public abstract class ExBulletSparkle: MonoBehaviour {
  protected WeaponManager weaponManager;
  protected Rigidbody2D rb;

  protected virtual void Awake() {
    rb = GetComponent<Rigidbody2D>();
    weaponManager = FindFirstObjectByType<WeaponManager>();
  }

  /// <summary>
  /// This method runs whenever the ExplosionAnimation of the bullet ends.
  /// </summary>
  public virtual void OnExBulletSfxAnimationEnd() {
    weaponManager.ReturnExSfx(gameObject);
  }
}
