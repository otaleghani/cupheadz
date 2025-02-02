using UnityEngine;

public class ChaserBullet : Bullet {
  public Transform target;          // The target to home in on
  public float turnSpeed = 200f;    // Speed at which the bullet turns towards the target

  protected override void FixedUpdate() {
    base.FixedUpdate(); // Handle lifetime and movement

    if (target != null) {
      Vector2 directionToTarget = ((Vector2)target.position - rb.position).normalized;
      Vector2 newDirection = Vector2.MoveTowards(rb.linearVelocity, directionToTarget, turnSpeed * Time.deltaTime);
      rb.linearVelocity = newDirection * speed;
    }
  }
}

