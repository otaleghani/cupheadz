using UnityEngine;

public class EnergyBeamManager : MonoBehaviour {
  private float damage = 50f;
  private float timer = 0.1f;
  private float counter;

  public void OnAnimationEnd() {
    counter = timer;
    Destroy(gameObject);
  }

  public void ActivateCollider() {
    Debug.Log("Activate the collider here");
  }
  private void OnTriggerStay2D(Collider2D other) {
    Debug.Log("helo");

    IDamageable damegeable = other.GetComponent<IDamageable>();
    if (damegeable != null) {
      counter -= Time.deltaTime;
      if (counter <= 0) {
        Debug.Log("Damageable takes damage!");
        damegeable.TakeDamage(damage);
        counter = timer;
        // Here I should play the sparkle animation
      }
    }
  }
}
