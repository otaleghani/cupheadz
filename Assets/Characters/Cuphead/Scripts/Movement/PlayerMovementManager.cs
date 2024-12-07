using UnityEngine;

public class PlayerMovementManager : MonoBehaviour {
  [Header("General")]
  [SerializeField] private bool isFacingRight = true;
  [SerializeField] private bool isLocked = false;
  public bool isGrounded = false;
  public bool isJumping;
  
  [Header("Movement")]
  [SerializeField] private float movementSpeed = 4f;
  [SerializeField] private float movementAcceleration = 0f;

  [Header("Jump")]
  [SerializeField] private float jumpStateMinTime = 0.05f;
  [SerializeField] private float jumpStateMaxTime = 0.4f;
  [SerializeField] private float jumpForce = 5.0f;
  private bool isJumpActionHeld;
  private float jumpTimeCounter;
  public bool isJumpReset;

  [Header("Dash")]
  [SerializeField] private float dashSpeed = 10f;
  [SerializeField] private float dashCooldown;
  [SerializeField] private float dashMaxCooldown = 1f;
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
    inputManager.OnJumpPerformed += HandleOnJumpPerformed;
    inputManager.OnJumpCanceled += HandleOnJumpCanceled;
    inputManager.OnMovePerformed += HandleOnMovePerformed;
    inputManager.OnMoveCanceled += HandleOnMoveCanceled;
    inputManager.OnLockPerformed += HandleOnLockPerformed;
    inputManager.OnLockCanceled += HandleOnLockReleased;
    inputManager.OnDashPerformed += HandleOnDashPerformed;
  }
  void OnDisable() {
    inputManager.OnJumpPerformed -= HandleOnJumpPerformed;
    inputManager.OnJumpCanceled -= HandleOnJumpCanceled;
    inputManager.OnMovePerformed -= HandleOnMovePerformed;
    inputManager.OnMoveCanceled -= HandleOnMoveCanceled;
    inputManager.OnLockPerformed -= HandleOnLockPerformed;
    inputManager.OnLockCanceled -= HandleOnLockReleased;
    inputManager.OnDashPerformed -= HandleOnDashPerformed;
  }

  void HandleOnJumpPerformed() {
    if (!isJumping && isGrounded) {
      isJumpActionHeld = true;
      isJumping = true;
      isGrounded = false;
      jumpTimeCounter = 0f;
    }
  }
  void HandleOnJumpCanceled() {
    isJumpActionHeld = false;
  }

  void HandleOnMovePerformed(Vector2 mov) {
    movementAcceleration = mov.x;
  }
  void HandleOnMoveCanceled() {
    movementAcceleration = 0f;
  }

  void HandleOnLockPerformed() {
    isLocked = true;
  }
  void HandleOnLockReleased() {
    isLocked = false;
  }

  void HandleOnDashPerformed() {
    isDashing = true;
  }

  public void FixedUpdate() {
    FlipCharacter();
    if (!isLocked 
        && stateManager.movementState is not PlayerCrouchState
        && stateManager.movementState is not PlayerLockState
    ) {
      Vector2 updatedPosition = rb.linearVelocity;

      if (isJumping && !isJumpReset) {
        updatedPosition = Jump(updatedPosition);
      }

      if (isDashing && !isDashingCooldown) {
        updatedPosition = Dash(updatedPosition);
      } else {
        // 
        updatedPosition = Move(updatedPosition);
      }
      if (isDashingCooldown) {
        dashCooldown -= Time.fixedDeltaTime;
      }
      if (dashCooldown <= 0) {
        isDashingCooldown = false;
      }

      rb.linearVelocity = updatedPosition;
    }
  }

  Vector2 Jump(Vector2 updatedPosition) {
    if (jumpTimeCounter <= jumpStateMinTime) {
      updatedPosition.y = jumpForce;
    }
    if (jumpTimeCounter <= jumpStateMaxTime && isJumpActionHeld) {
      updatedPosition.y = jumpForce;
    } else {
      isJumpReset = true;
      isJumping = false;
    }
    jumpTimeCounter += Time.fixedDeltaTime;
    return updatedPosition;
  }

  Vector2 Move(Vector2 updatedPosition) {
    updatedPosition.x = movementSpeed * movementAcceleration;
    return updatedPosition;
  }

  Vector2 Dash(Vector2 updatedPosition) {
    updatedPosition.x = dashSpeed;
    return updatedPosition;
  }

  void FlipCharacter() {
    if (isFacingRight && movementAcceleration < 0f ||
    !isFacingRight && movementAcceleration > 0f) {
      isFacingRight = !isFacingRight;
      Vector3 ls = transform.localScale;
      ls.x *= -1f;
      dashSpeed *= -1f;
      transform.localScale = ls;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    isGrounded = true;
    isJumping = false;
    isJumpReset = false;
  }

  public void OnDashingAnimationEnd() {
    isDashing = false;
    isDashingCooldown = true;
    dashCooldown = dashMaxCooldown;
    if (isGrounded) {
      stateManager.ChangeMovementState(new PlayerIdleState());
    } else {
      stateManager.ChangeMovementState(new PlayerJumpingState());
    }
  }
}
