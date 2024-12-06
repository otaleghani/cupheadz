using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private Animator animator;

  private int AnimatorMovementX;
  private int AnimatorMovementY;
  private int AnimatorIsRunning;
  private int AnimatorIsJumping;
  private int AnimatorIsCrouching;
  private int AnimatorIsCrouchingLoop;
  private int AnimatorIsDashing;

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    stateManager = GetComponent<PlayerStateManager>();

    animator = GetComponent<Animator>();
    AnimatorMovementX = Animator.StringToHash("xVelocity");
    AnimatorMovementY = Animator.StringToHash("yVelocity");
    AnimatorIsRunning = Animator.StringToHash("IsRunning");
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
    animator.SetBool(AnimatorIsRunning, false);
    animator.SetBool(AnimatorIsJumping, false);
  }

  public void ResetActionParameters() {
    // animator.SetBool(AnimatorIsShooting, false);
    // animator.SetBool(AnimatorIsTakingDamage, false);
    // animator.SetBool(AnimatorIsShootingEX, false);
  }

  public void SetParameterIsRunning() {
    animator.SetBool(AnimatorIsRunning, true);
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
    //animator.SetFloat(AnimatorMovementX, vector.x);
    //animator.SetFloat(AnimatorMovementY, vector.y);
    //if (vector.x != 0) {
    //  animator.SetBool(AnimatorIsMoving, true);
    //} else {
    //  animator.SetBool(AnimatorIsMoving, false);
    //}
  }

  void HandleOnMoveCanceled() {
    //animator.SetFloat(AnimatorMovementX, 0);
    //animator.SetFloat(AnimatorMovementY, 0);
    //animator.SetBool(AnimatorIsMoving, false);
  }

  public void OnCrouchingEntryEnd() {
    animator.SetBool(AnimatorIsCrouching, false);
    animator.SetBool(AnimatorIsCrouchingLoop, true);
  }
}
