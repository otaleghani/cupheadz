using UnityEngine;

public class PlayerMovementManager : MonoBehaviour {
  [Header("General")]
  public bool isFacingRight = true;
  
  [Header("Movement")]
  [SerializeField] private float movementSpeed = 6f;
  public int movementDirection = 0;

  [Header("Dash")]
  [SerializeField] private float dashSpeed = 6f;
  [SerializeField] private float dashCooldown;
  [SerializeField] public float dashMaxCooldown = 1f;
  public bool isDashingCooldown = false;
  public bool isDashing = false;

  [Header("Jump")]
  [SerializeField] private float maxJumpTime = 5f;
  [SerializeField] private float minJumpTime = 5f;
  private float jumpHoldTimer = 0f;
  private float jumpForce = 10f;
  private float jumpAcceleration = 0.1f;
  public bool isGrounded = false;
  public bool isJumping = false;
  public bool jumpHoldReleased = false;

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

  private void Move() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = movementSpeed * movementDirection;
    rb.linearVelocity = newPosition;
  }
  private void HandleMove() {
    if (stateManager.movementState is not PlayerAimState &&
        stateManager.movementState is not PlayerDashingState &&
        stateManager.movementState is not PlayerDeathState &&
        stateManager.movementState is not PlayerCrouchState) {
      Move();
    }
  }
  public void MoveStop() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = 0f;
    rb.linearVelocity = newPosition;
  }


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

  void HandleFlipCharacter() {
    if (stateManager.movementState is not PlayerDashingState) {
      if (isFacingRight && movementDirection < 0 ||
      !isFacingRight && movementDirection > 0) {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        dashSpeed *= -1f;
        transform.localScale = ls;
      }
    }
  }
  
  void FixedUpdate() {
    HandleDash();
    HandleJump();
    HandleMove();
    HandleFlipCharacter();
  }

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
  
  // Called whenever you enter a collision with a terrain
  private void OnTriggerEnter2D(Collider2D collision) {
    isGrounded = true;
    isJumping = false;
    jumpHoldReleased = true;
  }
}
