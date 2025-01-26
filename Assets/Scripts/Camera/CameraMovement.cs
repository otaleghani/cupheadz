using UnityEngine;

public class CameraMovement : MonoBehaviour {
  private GameObject player;
  private GameObject leftBound;
  private GameObject rightBound;

  public Vector3 offset = new Vector3(0, 0, -10);
  public Vector3 movementOffset = new Vector3(2, 0, 0);
  public float smoothSpeed = 0.5f;

  private Vector3 desiredOffset;
  private Vector3 currentOffset;

  private void Awake() {
    player = GameObject.Find("Cuphead");
    if (GameObject.Find("LeftBound") != null && GameObject.Find("RightBound") != null) {
      leftBound = GameObject.Find("LeftBound");
      rightBound = GameObject.Find("RightBound");
    } else {
      Destroy(this);
    }
  }
  
  private void LateUpdate() {
    if (player == null) {
      Debug.LogWarning("CameraFollow: No target assigned to follow.");
      return;
    }
    
    Vector3 dynamicOffset = Vector3.zero;
    if (player.GetComponent<PlayerInputManager>() != null) {
      if (player.GetComponent<PlayerInputManager>().xPosition > 0.1f && 
          player.GetComponent<PlayerStateManager>().movementState is not PlayerLockedState &&
          player.GetComponent<PlayerStateManager>().movementState is not PlayerCrouchState &&
          player.GetComponent<PlayerStateManager>().movementState is not PlayerAimState) {
        //dynamicOffset = movementOffset;
        dynamicOffset = Vector3.Lerp(currentOffset, new Vector3(-movementOffset.x, 0, 0), smoothSpeed);
      } else if (player.GetComponent<PlayerInputManager>().xPosition < -0.1f) {
        //dynamicOffset = new Vector3(-movementOffset.x, 0, 0);
        dynamicOffset = Vector3.Lerp(currentOffset, movementOffset, smoothSpeed);
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
