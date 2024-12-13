using UnityEngine;

public class MeatShieldManager : MonoBehaviour, IDamageable
{
  private float life;
  private new Collider2D collider;

  void Start() {
    collider = GetComponent<Collider2D>();
    this.life = 100f;
  }

  void Update() {}

  public void TakeDamage(float amount) {
    Debug.Log("got hit");
    this.life -= amount;
    if (this.life <= 0) {
      Destroy(gameObject);
    }
  }
}
