using UnityEngine;

public class PlayerMovementManager : MonoBehaviour {
  [Header("General")]
  [SerializeField] private bool isAimed = false;
  public bool isGrounded = false;
  public bool isJumping;
  public bool isFacingRight = true;
  
  [Header("Movement")]
  [SerializeField] private float movementSpeed = 4f;
  [SerializeField] private float movementAcceleration = 0f;
  [SerializeField] private int movementDirection = 0;
  public bool isMoving = false;

  [Header("Jump")]
  [SerializeField] private float jumpStateMinTime = 0.05f;
  [SerializeField] private float jumpStateMaxTime = 0.1f;
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
    inputManager.OnJumpPerformed += HandleOnJumpPerformed;
    inputManager.OnJumpCanceled += HandleOnJumpCanceled;
    inputManager.OnMovePerformed += HandleOnMovePerformed;
    inputManager.OnMoveCanceled += HandleOnMoveCanceled;
    inputManager.OnAimPerformed += HandleOnAimPerformed;
    inputManager.OnAimCanceled += HandleOnAimReleased;
    inputManager.OnDashPerformed += HandleOnDashPerformed;

    // new stuff
    inputManager.OnSerializedMovePerformed += HandleOnSerializedMovement;
    inputManager.OnSerializedMoveCanceled += HandleOnSerializedMovement;
  }
  void OnDisable() {
    inputManager.OnJumpPerformed -= HandleOnJumpPerformed;
    inputManager.OnJumpCanceled -= HandleOnJumpCanceled;
    inputManager.OnMovePerformed -= HandleOnMovePerformed;
    inputManager.OnMoveCanceled -= HandleOnMoveCanceled;
    inputManager.OnAimPerformed -= HandleOnAimPerformed;
    inputManager.OnAimCanceled -= HandleOnAimReleased;
    inputManager.OnDashPerformed -= HandleOnDashPerformed;

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
  public void Dash() {
    Vector2 newPosition = rb.linearVelocity;
    newPosition.x = dashSpeed * movementDirection;
    rb.linearVelocity = newPosition;
  }
  public void Jump() {
    Vector2 newPosition = rb.linearVelocity;
    if (jumpTimeCounter <= jumpStateMinTime) {
      newPosition.y = jumpForce;
    }
    if (jumpTimeCounter <= jumpStateMaxTime && isJumpActionHeld) {
      newPosition.y = jumpForce;
    } else {
      isJumpReset = true;
      isJumping = false;
    }
    jumpTimeCounter += Time.fixedDeltaTime;

    rb.linearVelocity = newPosition;

    // You call move here because you want to be able to move while jumping
    Move();
  }

  public void EndDashMovement() {
    isDashing = false;
    isDashingCooldown = true;
    dashCooldown = dashMaxCooldown;
  }
  public void EndJumpMovement() {
    isJumpActionHeld = false;
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

  // Called whenever you enter a collision with a terrain
  private void OnTriggerEnter2D(Collider2D collision) {
    isGrounded = true;
    isJumping = false;
    isJumpReset = false;
  }

  // OLD METHODS
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

  void HandleOnAimPerformed() {
    isAimed = true;
  }
  void HandleOnAimReleased() {
    isAimed = false;
  }

  void HandleOnDashPerformed() {
    isDashing = true;
  }

  public void FixedUpdate() {
    // The idea is that you don't want to have all of this if statements 
    // because you should already know in the state manager what is the
    // current state. You do not need to do something like this.
    //
    // You could centralize it here by using a switch statement but this
    // would just make it wierd, and also not so readable. Instead if you
    // have every single movement handler in the UpdateState() you can easily
    // pinpoint a bug 
    FlipCharacter();
    isMoving = false;

    if (!isAimed 
        && stateManager.movementState is not PlayerCrouchState
        && stateManager.movementState is not PlayerAimState
    ) {
      Vector2 updatedPosition = rb.linearVelocity;

      if (isJumping && !isJumpReset) {
        updatedPosition = Jump(updatedPosition);
      }

      if (isDashing && !isDashingCooldown) {
        updatedPosition = Dash(updatedPosition);
      } else {
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
    isMoving = true;
    return updatedPosition;
  }

  Vector2 Dash(Vector2 updatedPosition) {
    updatedPosition.x = dashSpeed;
    return updatedPosition;
  }


  //public void OnDashingAnimationEnd() {
  //  isDashing = false;
  //  isDashingCooldown = true;
  //  dashCooldown = dashMaxCooldown;
  //  if (isGrounded) {
  //    if (isMoving) {
  //      stateManager.ChangeMovementState(new PlayerMovingState());
  //    } else {
  //      stateManager.ChangeMovementState(new PlayerIdleState());
  //    }
  //  } else {
  //    stateManager.ChangeMovementState(new PlayerJumpingState());
  //  }
  //}

}
