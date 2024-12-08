using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private Animator animator;

  private int AnimatorXVelocity;
  private int AnimatorYVelocity;
  private int AnimatorIsMoving;
  private int AnimatorIsJumping;
  private int AnimatorIsCrouching;
  private int AnimatorIsCrouchingLoop;
  private int AnimatorIsDashing;
  private int AnimatorIsAiming;
  private int AnimatorXAim;
  private int AnimatorYAim;

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    stateManager = GetComponent<PlayerStateManager>();

    animator = GetComponent<Animator>();
    AnimatorXVelocity = Animator.StringToHash("xVelocity");
    AnimatorYVelocity = Animator.StringToHash("yVelocity");
    AnimatorIsMoving = Animator.StringToHash("IsMoving");
    AnimatorIsJumping = Animator.StringToHash("IsJumping");
    AnimatorIsCrouching = Animator.StringToHash("IsCrouching");
    AnimatorIsCrouchingLoop = Animator.StringToHash("IsCrouchingLoop");
    AnimatorIsDashing = Animator.StringToHash("IsDashing");
    AnimatorIsAiming = Animator.StringToHash("IsAiming");
    AnimatorXAim = Animator.StringToHash("xAim");
    AnimatorYAim = Animator.StringToHash("yAim");
  }

  void OnEnable() {
    inputManager.OnMovePerformed += HandleOnMovePerformed;
    inputManager.OnMoveCanceled += HandleOnMoveCanceled;
  }

  void OnDisable() {
    inputManager.OnMovePerformed -= HandleOnMovePerformed;
    inputManager.OnMoveCanceled -= HandleOnMoveCanceled;
  }

  public void ResetMovementParameters() {
    animator.SetBool(AnimatorIsCrouching, false);
    animator.SetBool(AnimatorIsCrouchingLoop, false);
    animator.SetBool(AnimatorIsJumping, false);
    animator.SetBool(AnimatorIsAiming, false);
  }

  public void ResetActionParameters() {
    // animator.SetBool(AnimatorIsShooting, false);
    // animator.SetBool(AnimatorIsTakingDamage, false);
    // animator.SetBool(AnimatorIsShootingEX, false);
  }

  public void SetParameterIsMoving() {
    animator.SetBool(AnimatorIsMoving, true);
  }

  public void SetParameterIsJumping() {
    animator.SetBool(AnimatorIsJumping, true);
  }

  public void SetParameterIsCrouching() {
    animator.SetBool(AnimatorIsCrouching, true);
  }

  public void SetParameterIsDashing() {
    animator.SetBool(AnimatorIsDashing, true);
  }

  public void SetParameterIsAiming() {
    animator.SetBool(AnimatorIsAiming, true);
  }
  public void SetParameterIsAimingDirection() {
    //animator.SetBool(AnimatorIsAimingDirection, true);
  }

  void HandleOnMovePerformed(Vector2 vector) {
    animator.SetFloat(AnimatorXVelocity, vector.x);
    animator.SetFloat(AnimatorYVelocity, vector.y);
    animator.SetInteger(AnimatorXAim, Mathf.RoundToInt(vector.x));
    animator.SetInteger(AnimatorYAim, Mathf.RoundToInt(vector.y));
    //animator.SetBool(AnimatorIsAimingDirection, true);
    //if (vector.x > 0) {
    //  animator.SetBool(AnimatorIsMoving, true);
    //}
  }
  void HandleOnMoveCanceled() {
    animator.SetFloat(AnimatorXVelocity, 0f);
    animator.SetFloat(AnimatorYVelocity, 0f);
    animator.SetInteger(AnimatorXAim, 0);
    animator.SetInteger(AnimatorYAim, 0);
    animator.SetBool(AnimatorIsMoving, false);
    //animator.SetBool(AnimatorIsAimingDirection, false);
  }

  public void OnCrouchingEntryEnd() {
    animator.SetBool(AnimatorIsCrouching, false);
    animator.SetBool(AnimatorIsCrouchingLoop, true);
  }
}
