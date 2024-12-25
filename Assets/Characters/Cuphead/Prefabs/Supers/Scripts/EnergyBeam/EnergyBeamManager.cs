using UnityEngine;

public class EnergyBeamManager : MonoBehaviour {
  private float damage = 5f;
  private float timer = 0.5f;
  private float counter;

  public void OnAnimationEnd() {
    Destroy(gameObject);
  }

  public void ActivateCollider() {}
  private void OnTriggerStay2D(Collider2D other) {
    IDamageable damegeable = other.GetComponent<IDamageable>();
    if (damegeable != null) {
      counter -= Time.deltaTime;
      if (counter <= 0) {
        damegeable.TakeDamage(damage);
        counter = timer;
        GameObject sparkle = Instantiate(Resources.Load<GameObject>("Super__EnergyBeamImpact"), BulletPoolManager.Instance.transform);
        sparkle.transform.position = other.transform.position;
      }
    }
  }
}
