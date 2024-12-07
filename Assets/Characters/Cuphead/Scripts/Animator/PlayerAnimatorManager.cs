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
    //animator.SetBool(AnimatorIsMoving, false);
    animator.SetBool(AnimatorIsJumping, false);
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

  public void SetParameterIstDashing() {
    animator.SetBool(AnimatorIsDashing, true);
  }




  void HandleOnMovePerformed(Vector2 vector) {
    animator.SetFloat(AnimatorXVelocity, vector.x);
    animator.SetFloat(AnimatorYVelocity, vector.y);
    animator.SetBool(AnimatorIsMoving, true);
    //animator.SetFloat(AnimatorXVelocity, vector.x);
    //animator.SetFloat(AnimatorMovementY, vector.y);
    //if (vector.x != 0) {
    //  animator.SetBool(AnimatorIsMoving, true);
    //} else {
    //  animator.SetBool(AnimatorIsMoving, false);
    //}
  }

  void HandleOnMoveCanceled() {
    animator.SetFloat(AnimatorXVelocity, 0f);
    animator.SetFloat(AnimatorYVelocity, 0f);
    animator.SetBool(AnimatorIsMoving, false);
    //animator.SetFloat(AnimatorMovementX, 0);
    //animator.SetFloat(AnimatorMovementY, 0);
    //animator.SetBool(AnimatorIsMoving, false);
  }

  public void OnCrouchingEntryEnd() {
    animator.SetBool(AnimatorIsCrouching, false);
    animator.SetBool(AnimatorIsCrouchingLoop, true);
  }
}
