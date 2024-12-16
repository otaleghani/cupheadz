using UnityEngine;
using System.Collections;

public class PlayerMovementManager : MonoBehaviour {
  [Header("General")]
  //[SerializeField] private bool isAimed = false;
  public bool isGrounded = false;
  public bool isJumping;
  public bool isFacingRight = true;
  
  [Header("Movement")]
  [SerializeField] private float movementSpeed = 4f;
  [SerializeField] private float movementAcceleration = 0f;
  [SerializeField] private int movementDirection = 0;
  public bool isMoving = false;

  [Header("Jump")]
  //[SerializeField] private float jumpStateMinTime = 0.1f;
  //[SerializeField] private float jumpStateMaxTime = 0.5f;
  [SerializeField] private float jumpForce = 5.0f;
  private bool isJumpActionHeld;
  private float jumpTimeCounter;
  public bool isJumpReset;

  [Header("Dash")]
  [SerializeField] private float dashSpeed = 10f;
  [SerializeField] private float dashCooldown;
  [SerializeField] public float dashMaxCooldown = 1f;
  public bool isDashingCooldown = false;
  public bool isDashing = false;

  private Rigidbody2D rb;
  private PlayerInputManager inputManager;
  private PlayerStateManager stateManager;

  void Awake() {
    rb = GetComponent<Rigidbody2D>();
    inputManager = GetComponent<PlayerInputManager>();
    stateManager = GetComponent<PlayerStateManager>();
  }

  void OnEnable() {
    inputManager.OnSerializedMovePerformed += HandleOnSerializedMovement;
    inputManager.OnSerializedMoveCanceled += HandleOnSerializedMovement;
  }
  void OnDisable() {
    inputManager.OnSerializedMovePerformed -= HandleOnSerializedMovement;
    inputManager.OnSerializedMoveCanceled -= HandleOnSerializedMovement;
  }

  void HandleOnSerializedMovement(int x, int y) {
    movementDirection = x;
  }
  public void Move() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = movementSpeed * movementDirection;
    rb.linearVelocity = newPosition;
  }
  public void Stop() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = 0f;
    rb.linearVelocity = newPosition;
  }

  public void Dash() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = dashSpeed * movementDirection;
    rb.linearVelocity = newPosition;
  }

  private Coroutine jumpCoroutine;
  private float smallJumpForce = 5f;
  private float bigJumpAdditionalForce = 5f;
  private float jumpMaxHeight = 2.5f;
  private float jumpMinHeight = 0.5f; 
  private float jumpTimeToMaxHeight = 0.4f;
  private float jumpStateMinTime = 0.05f;

  public void Jump() {
    if (!isJumping) {
      isJumping = true;

      rb.linearVelocity = new Vector2(rb.linearVelocity.x, smallJumpForce);
      if (jumpCoroutine != null) {
        StopCoroutine(jumpCoroutine);
      }
      jumpCoroutine = StartCoroutine(HandleJumpHold());
    }
    Move();
  }

  private IEnumerator HandleJumpHold() {
    float holdTime = 0f;

    while (holdTime < jumpStateMaxTime) {
      if (!isJumpActionHeld) {
        Debug.Log("helo");
        break;
      }
      holdTime += Time.deltaTime;

      if (holdTime >= jumpStateMinTime && rb.linearVelocity.y == smallJumpForce) {
        UpgradeToBigJump();
      }
      
      yield return null;
    }
    isJumping = false;
  }

  private void UpgradeToBigJump() {
    rb.linearVelocity = new Vector2(rb.linearVelocity.x, smallJumpForce + bigJumpAdditionalForce);
  }

  public void JumpStart() {
    jumpTimeCounter = 0f;
    isJumpActionHeld = true;
  }
  public void JumpReset() {
    jumpTimeCounter = 0f;
    isJumpActionHeld = false;
  }
  public void EndJumpMovement() {
    isJumpActionHeld = false;
  }

  public void EndDashMovement() {
    isDashing = false;
    isDashingCooldown = true;
    dashCooldown = dashMaxCooldown;
  }

  void FlipCharacter() {
    if (isFacingRight && movementDirection < 0 ||
    !isFacingRight && movementDirection > 0) {
      isFacingRight = !isFacingRight;
      Vector3 ls = transform.localScale;
      ls.x *= -1f;
      dashSpeed *= -1f;
      transform.localScale = ls;
    }
  }

  // Called whenever you enter a collision with a terrain
  private void OnTriggerEnter2D(Collider2D collision) {
    isGrounded = true;
    isJumping = false;
    isJumpReset = false;
    isJumpActionHeld = false;
  }

  private void FixedUpdate() {
    FlipCharacter();
  }
}
