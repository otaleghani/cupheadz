using UnityEngine;

public abstract class Bullet : MonoBehaviour {

  [Header("Bullet properties")]
  public float damage = 10f;
  public float speed = 20f;
  public float lifeTime = 5f;
  public float fireRate = 0.5f;

  public Rigidbody2D rb;
  public Vector2 direction = Vector2.right;

  protected float lifeTimer;

  protected virtual void Start() {
    rb = GetComponent<Rigidbody2D>();
    rb.linearVelocity = direction.normalized * speed;
    lifeTimer = lifeTime;
  }

  protected virtual void Update() {
    HandleLifeTimer();
    HandleMove();
  }

  protected virtual void HandleMove() {}

  protected virtual void HandleLifeTimer() {
    lifeTimer -= Time.deltaTime;
    if (lifeTimer <= 0f) {
      Destroy(gameObject);
    }
  }

  public virtual void FlipBullet() {
    Vector3 ls = transform.localScale;
    ls.x = -1f;
    transform.localScale = ls;
  }
}
