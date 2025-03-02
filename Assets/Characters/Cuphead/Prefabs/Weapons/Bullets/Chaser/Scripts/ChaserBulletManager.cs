using UnityEngine;
public class ChaserBullet : Bullet {

  protected override void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    weaponManager = GameObject.Find("Chaser__Weapon").GetComponent<WeaponManager>();
  }
  private Transform target;          // The target to home in on
  private Rigidbody2D targetRb;
  private Collider2D targetCol;
  private float turnSpeed = 1f;    // Speed at which the bullet turns towards the target
  private float rotateSpeed = 2000f;
  private float lerpFactor = 0.05f;

  protected override void FixedUpdate() {
    base.FixedUpdate(); // Handle lifetime and movement

    if (target != null) {
      Vector2 targetPos = targetCol.bounds.center;
      Vector3 direction = (targetPos - rb.position).normalized;

      // Rotate the missile to face the target direction smoothly
      Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 0.0f);
      rb.MoveRotation(Quaternion.LookRotation(newDirection));
      
      Vector3 desiredVelocity = newDirection * speed * turnSpeed;
      rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, lerpFactor);
      
    } else {
      target = FindNearestEnemy().transform;
      targetRb = target.GetComponent<Rigidbody2D>();
      targetCol = target.GetComponent<Collider2D>();
    }
  }

  private GameObject FindNearestEnemy() {
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
