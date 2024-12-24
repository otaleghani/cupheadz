using UnityEngine;

/// <summary>
/// Manages the ExBullet prefab, so it's speed, damage, lifetime and collisions
/// </summary>
public abstract class ExBullet : MonoBehaviour {
  [Header("Bullet properties")]
  public virtual float damage { get; protected set; } = 10f;
  public virtual float speed { get; protected set; } = 20f;
  public virtual float lifeTime { get; protected set; } = 40f;
  private float lifeTimer;

  public Rigidbody2D rb;
  public Animator animator;
  public Vector2 direction = Vector2.right;
  protected WeaponManager weaponManager;

  protected virtual void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    weaponManager = FindFirstObjectByType<WeaponManager>();
  }
  private void OnEnable() {
    lifeTimer = lifeTime;
  }

  protected virtual void FixedUpdate() {
    HandleLifeTimer();
    HandleMove();
  }

  /// <summary>
  /// By default does nothing.
  /// Use this to add additional movement to the bullet, like a bounce or chaseing mechanic
  /// </summary>
  protected virtual void HandleMove() {}

  /// <summary>
  /// By default subtracts every frame the bullet time, and returns it at the end.
  /// Use this to add additional actions at the end, like an explosion.
  /// </summary>
  protected virtual void HandleLifeTimer() {
    lifeTimer -= Time.deltaTime;
    if (lifeTimer <= 0f) {
      weaponManager.ReturnExBullet(gameObject);
    }
  }

  /// <summary>
  /// By default deals damage to the other collider, stops the bullet and plays explode animation.
  /// Use this to add additional actions to the collider, like a damage over time.
  /// </summary>
  protected virtual void HandleCollision(Collider2D other) {
    if (other.CompareTag("Player")){
      return;
    }
    IDamageable damageable = other.GetComponent<IDamageable>();
    if (damageable != null) {
      damageable.TakeDamage(damage);
      animator.SetBool("MadeContact", true);
    } else {
      animator.SetBool("MadeContact", true);
    }
    rb.linearVelocityX = 0f;
    rb.linearVelocityY = 0f;
  }

  /// <summary>
  /// This method runs whenever the ExplosionAnimation of the bullet ends.
  /// </summary>
  protected virtual void OnExplosionAnimationEnd() {
    weaponManager.ReturnExBullet(gameObject);
  }

  protected virtual void OnTriggerEnter2D(Collider2D other) {
    HandleCollision(other);
  }
}
