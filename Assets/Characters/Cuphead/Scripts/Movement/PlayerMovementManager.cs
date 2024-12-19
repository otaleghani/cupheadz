using UnityEngine;

/// <summary>
/// Handles the movement of the Player, so run, jump and dash. Every single movement has at least
/// two methods. One is called every fixed frame to check if the action is to be performed, and
/// the other performs the action. Sometimes you'll also find methods that are call at the end
/// to do some cleanup.
/// </summary>
public class PlayerMovementManager : MonoBehaviour {
  [Header("General")]
  public bool isFacingRight = true;
  
  [Header("Movement")]
  [SerializeField] private float movementSpeed = 6f;

  [Header("Dash")]
  [SerializeField] private float dashSpeed = 6f;
  [SerializeField] private float dashCooldown;
  public float dashMaxCooldown = 1f;
  public bool isDashingCooldown = false;
  public bool isDashing = false;

  [Header("Jump")]
  [SerializeField] private float maxJumpTime = 0.2f;
  [SerializeField] private float minJumpTime = 0.05f;
  [SerializeField] private float jumpHoldTimer = 0f;
  [SerializeField] private float jumpForce = 10f;
  [SerializeField] private float jumpAcceleration = 0.1f;
  public bool isGrounded = true;
  public bool isJumping = false;
  public bool jumpHoldReleased = false;

  private Rigidbody2D rb;
  private PlayerInputManager inputManager;
  private PlayerStateManager stateManager;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    inputManager = GetComponent<PlayerInputManager>();
    stateManager = GetComponent<PlayerStateManager>();
  }

  private void FixedUpdate() {
    HandleDash();
    HandleJump();
    HandleMove();
    HandleFlipCharacter();
  }

  /// <summary>
  /// Movement scripts
  /// </summary>
  private void HandleMove() {
    if (stateManager.movementState is not PlayerAimState &&
        stateManager.movementState is not PlayerDashingState &&
        stateManager.movementState is not PlayerDeathState &&
        stateManager.movementState is not PlayerCrouchState) {
      Move();
    }
  }
  private void Move() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = movementSpeed * inputManager.xPosition;
    rb.linearVelocity = newPosition;
  }
  public void MoveStop() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = 0f;
    rb.linearVelocity = newPosition;
  }


  /// <summary>
  /// Dash scripts
  /// </summary>
  private void HandleDash() {
    // Keeps track of the cooldown for the dash
    if (isDashingCooldown) {
      dashCooldown -= Time.deltaTime;
      if (dashCooldown <= 0) {
        isDashingCooldown = false;
      }
    }
    if (isDashing) {
      Dash();
    }
  }
  private void Dash() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = dashSpeed;
    rb.linearVelocity = newPosition;
  }
  public void EndDash() {
    isDashing = false;
    isDashingCooldown = true;
    dashCooldown = dashMaxCooldown;
  }

  /// <summary>
  /// Character flip
  /// </summary>
  void HandleFlipCharacter() {
    if (stateManager.movementState is not PlayerDashingState) {
      if (isFacingRight && inputManager.xPosition < 0 ||
      !isFacingRight && inputManager.xPosition > 0) {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        dashSpeed *= -1f;
        transform.localScale = ls;
      }
    }
  }

  /// <summary>
  /// Jump scripts
  /// </summary>
  private void HandleJump() {
    if (isJumping) {
      jumpHoldTimer += Time.deltaTime;
      Jump();
      if (jumpHoldReleased && jumpHoldTimer >= minJumpTime) {
        EndJump();
      }
      if (jumpHoldTimer >= maxJumpTime) {
        EndJump();
      }
    }
  }
  public void StartJump() {
    rb.gravityScale = 0;
    isJumping = true;
    isGrounded = false;
    jumpHoldTimer = 0f;
    jumpHoldReleased = false;
  }
  private void Jump() {
    Debug.Log("Jump");
    Vector2 newVelocity = rb.linearVelocity;
    newVelocity.y = jumpForce + (jumpHoldTimer * jumpAcceleration);
    rb.linearVelocity = newVelocity;
  }
  private void EndJump() {
    isJumping = false;
    rb.gravityScale = 3;
  }
  
  /// <summary>
  /// Collide with ground
  /// </summary>
  private void OnTriggerEnter2D(Collider2D collision) {
    // todo: check if the trigger collider was actually the ground or not
    isGrounded = true;
    isJumping = false;
    jumpHoldReleased = true;
  }
}
