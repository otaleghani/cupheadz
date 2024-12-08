using UnityEngine;

/// WIP
public class HomingBullet : Bullet {
  public Transform target;          // The target to home in on
  public float turnSpeed = 200f;    // Speed at which the bullet turns towards the target

  protected override void Update() {
    base.Update(); // Handle lifetime and movement

    if (target != null) {
      Vector2 directionToTarget = ((Vector2)target.position - rb.position).normalized;
      //Vector2 newDirection = Vector2.RotateTowards(rb.linearVelocity.normalized, directionToTarget, turnSpeed * Time.deltaTime, 0f);
      //rb.linearVelocity = newDirection * speed;
    }
  }
}

