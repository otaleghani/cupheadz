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
  public Collider2D currentGround;
  
  [Header("Movement")]
  private float movementSpeed = 6f / 1.5f;

  [Header("Dash")]
  private float dashSpeed = 15f / 1.5f;
  private float dashCooldown;
  private float dashMaxCooldown = 0.05f; // OLD_VALUES: 0.5
  public bool isDashingCooldown = false;
  public bool isDashing = false;

  [Header("Jump")]
  private float maxJumpTime = 0.20f; // OLD_VALUES: 0.27
  private float minJumpTime = 0.15f; // OLD_VALUES: 0.15 | 0.19 | 0.21
  private float jumpTransform = 0.28f / 1.5f; // OLD_VALUES: 0.37  | 0.45 | 0.18 |
  private float jumpForce = 1.1f;
  private float ascendingGravity = 1f;
  private float descendingGravity = 5f;

  private float jumpHoldTimer = 0f;
  //private float jumpForce = 12f;
  //private float jumpAcceleration = 2f;
  public bool isGrounded = true;
  public bool isJumping = false;
  public bool jumpHoldReleased = false;

  private Rigidbody2D rb;
  private PlayerInputManager inputManager;
  private PlayerStateManager stateManager;

  private PlayerGroundCollision groundCollision;

  private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    inputManager = GetComponent<PlayerInputManager>();
    stateManager = GetComponent<PlayerStateManager>();
    groundCollision = GetComponentInChildren<PlayerGroundCollision>();

    movementSpeed *= rb.transform.localScale.x;
    dashSpeed *= rb.transform.localScale.x;
    jumpTransform *= rb.transform.localScale.x;
  }

  private void OnEnable() {
    groundCollision.OnGroundCollisionEnter += HandleGroundCollisionEnter;
    groundCollision.OnGroundCollisionExit += HandleGroundCollisionExit;
  }
  private void OnDisable() {
    groundCollision.OnGroundCollisionEnter -= HandleGroundCollisionEnter;
    groundCollision.OnGroundCollisionExit -= HandleGroundCollisionExit;
  }

  private void HandleGroundCollisionEnter(Collider2D collision) {
    isGrounded = true;
    isJumping = false;
    jumpHoldReleased = true;
    currentGround = collision;
  }
  private void HandleGroundCollisionExit(Collider2D collision) {
    if (collision == currentGround) {
      isGrounded = false;
      currentGround = null;
    }
  }

  private void FixedUpdate() {
    HandleDash();
    HandleMove();
    HandleJump();
    HandleFlipCharacter();
    HandleExRecoil();
  }

  private float exRecoilTimer;
  private bool isRecoiling;
  private float maxRecoilTime = 0.1f;
  public void ExRecoil() {
    exRecoilTimer = 0f;
    isRecoiling = true;
  }
  private void HandleExRecoil() {
    if (isRecoiling) {
      ReleaseHoldPosition();
      Vector2 newPosition = rb.linearVelocity;
      if (isFacingRight) {
        newPosition.x = movementSpeed * -1f;
      } else {
        newPosition.x = movementSpeed;
      }
      rb.linearVelocity = newPosition;
      exRecoilTimer += Time.deltaTime;
      if (exRecoilTimer > maxRecoilTime) {
        isRecoiling = false;
      }
    }
  }

  /// <summary>
  /// Helper function used to stop the player character in the current position.
  /// </summary>
  public void HoldPosition() {
    // rb.gravityScale = 0f;
    rb.linearVelocity = new Vector2(0f, 0f);
    rb.constraints = RigidbodyConstraints2D.FreezeAll;
    if (isJumping) EndJump();
  }
  public void ReleaseHoldPosition() {
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
  }
  public void HoldYPosition() {
    //rb.gravityScale = 0f;
    rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
  }

  /// <summary>
  /// Movement scripts
  /// </summary>
  private void HandleMove() {
    if (stateManager.movementState is not PlayerAimState &&
        stateManager.movementState is not PlayerDashingState &&
        stateManager.movementState is not PlayerDeathState &&
        stateManager.movementState is not PlayerCrouchState &&
        stateManager.actionState is not PlayerDamagedState &&
        stateManager.actionState is not PlayerDeathState) {
      Move();
    }
  }
  private void Move() {
    //Vector2 newPosition = rb.linearVelocity;
    //newPosition.x = movementSpeed * inputManager.xPosition;
    //rb.linearVelocity = newPosition;
    rb.linearVelocityX = movementSpeed * inputManager.xPosition;
    //Vector3 newPosition = rb.transform.localPosition;
    //newPosition.x += (movementSpeed * inputManager.xPosition) / 30;
    //rb.transform.localPosition = newPosition;
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
      EndJump();
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
    rb.gravityScale = ascendingGravity;
    isJumping = true;
    isGrounded = false;
    jumpHoldTimer = 0f;
    jumpHoldReleased = false;
    
    jumpStartY = rb.transform.localPosition.y;
  }
  
  // Old jump
  
  private void Jump() {
    rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    Vector3 newPosition = rb.transform.localPosition;
    // newPosition.y += jumpTransform - (jumpHoldTimer / 2);
    newPosition.y += jumpTransform;
    rb.transform.localPosition = newPosition;
  }
  
  // New jump
  float jumpStartY;
  // public AnimationCurve jumpCurve;
  // private void Jump() {
  //   float t = Mathf.Lerp(minJumpTime, maxJumpTime, jumpHoldTimer);
    
  //   // float easeOutValue = -t * t + 2 * t;
  //   // float easeOutValue = 1 - Mathf.Pow(1 - t, 3);
  //   // float currentJumpOffset = jumpTransform * easeOutValue;

  //   float curveValue = jumpCurve.Evaluate(t);
  //   float currentJumpOffset = jumpTransform + (jumpTransform * curveValue);

  //   Vector3 newPosition = rb.transform.localPosition;
  //   newPosition.y = jumpStartY + currentJumpOffset;
  //   rb.transform.localPosition = newPosition;
  // }
  
  private void EndJump() {
    rb.gravityScale = descendingGravity;
    isJumping = false;
  }

  public void DisableCollider() { 
    //rb.simulated = false; 
    this.GetComponent<Collider2D>().enabled = false;
  }
  public void EnableCollider() {
    //rb.simulated = true; 
    this.GetComponent<Collider2D>().enabled = true;
  }
}
