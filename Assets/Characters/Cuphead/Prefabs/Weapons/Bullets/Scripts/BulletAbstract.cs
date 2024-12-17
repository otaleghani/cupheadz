using UnityEngine;

public abstract class Bullet : MonoBehaviour {
  [Header("Bullet properties")]
  public float damage = 10f;
  public float speed = 20f;
  public float lifeTime = 2f;

  public Rigidbody2D rb;
  public Animator animator;
  public Vector2 direction = Vector2.right;

  protected float lifeTimer;

  private WeaponManager weaponManager;

  protected virtual void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    lifeTimer = lifeTime;
    weaponManager = GetComponentInParent<WeaponManager>();
  }

  protected virtual void Update() {
    HandleLifeTimer();
    HandleMove();
  }

  protected virtual void HandleMove() {}

  protected virtual void HandleLifeTimer() {
    lifeTimer -= Time.deltaTime;
    if (lifeTimer <= 0f) {
      weaponManager.ReturnBullet(gameObject);
    }
  }

  public virtual void FlipBullet() {
    Vector3 ls = transform.localScale;
    ls.x = -1f;
    transform.localScale = ls;
  }

  protected virtual void OnTriggerEnter2D(Collider2D other) {
    HandleCollision(other);
  }

  protected virtual void HandleCollision(Collider2D other) {
    IDamageable damageable = other.GetComponent<IDamageable>();
    if (damageable != null) {
      damageable.TakeDamage(damage);
      animator.SetBool("MadeContact", true);
    }
    else {
      animator.SetBool("MadeContact", true);
    }
    rb.linearVelocityX = 0f;
    rb.linearVelocityY = 0f;
  }

  protected virtual void OnExplosionAnimationEnd() {
    weaponManager.ReturnBullet(gameObject);
  }
}
