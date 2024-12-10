using UnityEngine;

public class PeashooterManager : Bullet {
  protected virtual void OnTriggerEnter2D(Collider2D other) {
    HandleCollision(other);
  }

  protected virtual void HandleCollision(Collider2D other) {
    IDamageable damageable = other.GetComponent<IDamageable>();
    if (damageable != null) {
      damageable.TakeDamage(damage);
      Destroy(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }
}
