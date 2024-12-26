using UnityEngine;

public class GiantGhostManager : MonoBehaviour {
  private Animator animator;

  private float damage = 4f;
  private float timerSlow = 0.2f;
  private float timerFast = 0.1f;
  private float movementSpeed = 10f;
  private float counter = 0f;
  private float timer;
  private GameObject target;
  private bool spawned = false;
  private float yOffset = -1.5f;
  private new Collider2D collider;

  private void Awake() {
    collider = GetComponent<Collider2D>();
    animator = GetComponent<Animator>();
    counter = 0f;
    target = FindNearestEnemy();
    collider.enabled = false;
  }

  // Function called on animation end to progress
  public void OnSpawnEnd() {
    animator.Play("GiantGhostLoop");
    spawned = true;
    collider.enabled = true;
  }
  public void OnAttackEnd() {
    animator.Play("GiantGhostLoopFast");
    timer = timerFast;
  }
  public void OnAttackFastEnd() {
    animator.Play("GiantGhostExplosion");
    spawned = false;
  }
  public void OnExplosionEnd() {
    Destroy(gameObject);
  }

  private void FixedUpdate() {
    if (!spawned) return;
    if (target != null) {
      float step = movementSpeed * Time.deltaTime;
      Vector3 targetPosition = new Vector3(
        target.transform.position.x,
        target.transform.position.y + yOffset,
        target.transform.position.z
      );
      transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    } else {
      target = FindNearestEnemy();
    }
  }

  private void OnTriggerStay2D(Collider2D other) {
    IDamageable damegeable = other.GetComponent<IDamageable>();
    if (damegeable != null) {
      counter -= Time.deltaTime;
      if (counter <= 0) {
        damegeable.TakeDamage(damage);
        counter = timer;
      }
    }
  }

  GameObject FindNearestEnemy() {
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    GameObject nearest = null;
    float minDist = Mathf.Infinity;
    Vector3 currentPos = transform.localPosition;

    foreach (GameObject enemy in enemies) {
      float dist = Vector3.Distance(enemy.transform.position, currentPos);
      if (dist < minDist) {
        minDist = dist;
        nearest = enemy;
      }
    }
    return nearest;
  }
}
