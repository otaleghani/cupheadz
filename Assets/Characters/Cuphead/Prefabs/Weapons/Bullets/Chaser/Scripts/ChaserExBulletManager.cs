using UnityEngine;
using System.Collections.Generic;

public class ChaserExBullet : ExBullet {
  private bool _isFired;      // If the second bullet is fired
  private Transform _cuphead;
  private Transform target;          // The target to home in on
  private Rigidbody2D targetRb;
  private Collider2D targetCol;
  private float orbitSpeed = 20f;
  private float orbitRadius = 1.2f;
  
  private float turnSpeed = 1f;    // Speed at which the bullet turns towards the target
  private float rotateSpeed = 10000f;
  private float lerpFactor = 0.5f;
  
  public float currentAngle = 0f;

  protected override void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    weaponManager = GameObject.Find("Chaser__Weapon").GetComponent<WeaponManager>();
    _cuphead = GameObject.Find("Cuphead").transform;
  }

  protected override void OnEnable() {
    base.OnEnable();
    _cuphead.GetComponent<PlayerInputManager>().OnShootEXPerformed += HandleExPerformed;  
  }
  
  private void OnDisable() {
    _cuphead.GetComponent<PlayerInputManager>().OnShootEXPerformed -= HandleExPerformed;  
  }
  private void HandleExPerformed() {
    _isFired = true;
  }

  // Add orbital with conditional for is fired
  protected override void HandleMove() {
    rb.linearVelocity = Vector2.zero;
    if (!_isFired) {
      // transform.RotateAround(_cuphead.position, Vector3.forward, orbitSpeed * Time.deltaTime);
      
      // Update the angle based on orbit speed and deltaTime
      currentAngle += orbitSpeed;

      // Convert the angle to radians for Mathf.Sin and Mathf.Cos
      float angleRad = currentAngle * Mathf.Deg2Rad;

      // Calculate the new offset position on a circle in the XY plane
      float offsetX = Mathf.Cos(angleRad) * orbitRadius;
      float offsetY = Mathf.Sin(angleRad) * orbitRadius;

      // Set the object's position relative to the pivot's current position
      transform.position = new Vector3(_cuphead.position.x + offsetX, _cuphead.position.y + 0.5f + offsetY, transform.position.z);     
    } else {
      FollowEnemy();
    }
  }

  private void FollowEnemy() {
    if (targetCol != null) {
      Vector2 targetPos = targetCol.bounds.center;
      Vector3 direction = (targetPos - rb.position).normalized;

      // Rotate the missile to face the target direction smoothly
      Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 0.0f);
      rb.MoveRotation(Quaternion.LookRotation(newDirection));
      
      Vector3 desiredVelocity = newDirection * speed * turnSpeed;
      rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, lerpFactor);
    } else {
      GameObject targetGameObject = FindNearestEnemy();
      if (targetGameObject == null) return;
      if (targetGameObject.transform != null) {
        target = FindNearestEnemy().transform;
        targetRb = target.GetComponent<Rigidbody2D>();
        targetCol = target.GetComponent<Collider2D>();
      }
    }
  }
  
  private GameObject FindNearestEnemy() {
    List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    enemies.AddRange(GameObject.FindGameObjectsWithTag("EnemyUnreachable"));
    
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

  // Reset
  protected override void OnExplosionAnimationEnd() {
    _isFired = false;
    base.OnExplosionAnimationEnd();
  }
}
