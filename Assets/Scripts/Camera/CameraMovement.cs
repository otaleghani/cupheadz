using UnityEngine;

public class CameraMovement : MonoBehaviour {
  public GameObject player;
  public GameObject leftBound;
  public GameObject rightBound;

  public Vector3 offset = new Vector3(0, 0, -10);
  public Vector3 movementOffset = new Vector3(-2, 0, 0);
  public float smoothSpeed = 0.125f;

  private Vector3 desiredOffset;
  private Vector3 currentOffset;

  private void LateUpdate() {
    if (player == null) {
      Debug.LogWarning("CameraFollow: No target assigned to follow.");
      return;
    }

    Vector3 dynamicOffset = Vector3.zero;
    if (player.GetComponent<Rigidbody2D>() != null) {
      Vector2 velocity = player.GetComponent<Rigidbody2D>().linearVelocity;
      if (velocity.x > 0.1f) {
        dynamicOffset = movementOffset;
      } else if (velocity.x < -0.1f) {
        dynamicOffset = new Vector3(-movementOffset.x, 0, 0);
      } else {
        dynamicOffset = Vector3.Lerp(currentOffset - offset, Vector3.zero, smoothSpeed);
      }
    }
    currentOffset = Vector3.Lerp(currentOffset, dynamicOffset, smoothSpeed);
    Vector3 desiredPosition = player.transform.position + offset + currentOffset;
    if (leftBound != null && rightBound != null) {
      desiredPosition.x = Mathf.Clamp(desiredPosition.x, leftBound.transform.position.x, rightBound.transform.position.x);
    }
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    smoothedPosition.y = 0f;
    smoothedPosition.z = -10f;
    transform.position = smoothedPosition;
  }
}
