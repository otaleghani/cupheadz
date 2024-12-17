using UnityEngine;
using System.Collections;

public class PlayerMovementManager : MonoBehaviour {
  [Header("General")]
  public bool isFacingRight = true;
  
  [Header("Movement")]
  [SerializeField] private float movementSpeed = 4f;
  [SerializeField] private float movementAcceleration = 0f;
  [SerializeField] private int movementDirection = 0;
  public bool isMoving = false;


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
    //Vector2 newPosition = rb.linearVelocity;
    //newPosition.x = dashSpeed * movementDirection;
    //rb.linearVelocity = newPosition;
    rb.AddForceX(dashSpeed * movementDirection, ForceMode2D.Impulse);
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
    //isJumpReset = false;
    //isJumpActionHeld = false;
  }


  // New jump
  
  [Header("Jump")]
  [Tooltip("Full jump: Maximum height of the jump in units.")]
  public float fullJumpHeight = 2f;

  [Tooltip("Full jump: Time to reach max height in seconds.")]
  public float fullJumpTime = 0.5f;

  [Tooltip("Short jump: Time before releasing jump button affects jump height.")]
  public float shortJumpMaxHoldTime = 0.25f;

  private float gravity;
  private float initialJumpVelocity;
  private float jumpHoldTimer;
  public bool isGrounded = false;
  public bool isJumping;
  public bool jumpHoldReleased = false;

  void Start() {
    // Calculates gravity based on full jump parameters
    gravity = (2 * fullJumpHeight) / Mathf.Pow(fullJumpTime, 2);
    
    // Gravity positive is down, so set it negative
    gravity = -gravity;

    // Calculates initial jump velocity for full jump
    initialJumpVelocity = Mathf.Abs(gravity) * fullJumpTime;

    // Disables Unitu's build-in gravity
    rb.gravityScale = 0;

    //rb.linearVelocity = Vector2.zero;
  }

  void Update() {
    if (isJumping) {
      jumpHoldTimer += Time.deltaTime;

      if (jumpHoldReleased || jumpHoldTimer >= fullJumpTime) {
        EndJump();
      }
    }
  }

  public void StartJump() {
    isJumping = true;
    jumpHoldTimer = 0f;
    jumpHoldReleased = false;
    rb.linearVelocity = new Vector2(rb.linearVelocityX, initialJumpVelocity);
  }
  void EndJump() {
    isJumping = false;
    if (rb.linearVelocityX > 0) {
      rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
    }
  }
  void ApplyCustomGravity() {
    rb.linearVelocity += Vector2.up * gravity * Time.fixedDeltaTime;
  }
  private void FixedUpdate() {
    ApplyCustomGravity();
    FlipCharacter();
  }
}
