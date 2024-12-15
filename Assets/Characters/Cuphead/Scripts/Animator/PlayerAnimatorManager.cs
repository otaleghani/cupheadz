using UnityEngine;
using System.Collections.Generic;

public class PlayerAnimatorManager : MonoBehaviour {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private Animator animator;

  public enum PlayerAnimations {
    Idle,
    Running,
    Jumping,
    Crouching,
    CrouchingLoop,
    Dashing,
    AimingFront,
    AimingUp,
    AimingDown,
    AimingDiagonalUp,
    AimingDiagonalDown,
    ShootingFront,
    ShootingUp,
    ShootingDown,
    ShootingDiagonalUp,
    ShootingDiagonalDown,
  }

  private Dictionary<PlayerAnimations, string> animations = new Dictionary<PlayerAnimations, string>();
  private Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> aimAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();
  private Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> shootAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();

  //private int AnimatorXVelocity;
  //private int AnimatorYVelocity;
  //private int AnimatorIsMoving;
  //private int AnimatorIsJumping;
  //private int AnimatorIsCrouching;
  //private int AnimatorIsCrouchingLoop;
  //private int AnimatorIsDashing;
  //private int AnimatorIsAiming;
  //private int AnimatorXAim;
  //private int AnimatorYAim;
  //private int AnimatorIsShooting;

  private PlayerAnimations currentAnimation = PlayerAnimations.Idle;

  public void ChangeAnimation(PlayerAnimations animation) {
    if (currentAnimation != animation) {
      animator.Play(animations[animation]);
      currentAnimation = animation;
    }
  }

  void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    stateManager = GetComponent<PlayerStateManager>();

    animator = GetComponent<Animator>();
    //AnimatorXVelocity = Animator.StringToHash("xVelocity");
    //AnimatorYVelocity = Animator.StringToHash("yVelocity");
    //AnimatorIsMoving = Animator.StringToHash("IsMoving");
    //AnimatorIsJumping = Animator.StringToHash("IsJumping");
    //AnimatorIsCrouching = Animator.StringToHash("IsCrouching");
    //AnimatorIsCrouchingLoop = Animator.StringToHash("IsCrouchingLoop");
    //AnimatorIsDashing = Animator.StringToHash("IsDashing");
    //AnimatorIsAiming = Animator.StringToHash("IsAiming");
    //AnimatorXAim = Animator.StringToHash("xAim");
    //AnimatorYAim = Animator.StringToHash("yAim");
    //AnimatorIsShooting = Animator.StringToHash("IsShooting");

    animations[PlayerAnimations.Idle] = "Cuphead__Idle";
    //firePoints[PlayerInputManager.AimDirection.Up] = transform.Find("Up");
    //firePoints[PlayerInputManager.AimDirection.Down] = transform.Find("Down");
    //firePoints[PlayerInputManager.AimDirection.Front] = transform.Find("Front");
    //firePoints[PlayerInputManager.AimDirection.DiagonalUp] = transform.Find("DiagonalUp");
    //firePoints[PlayerInputManager.AimDirection.DiagonalDown] = transform.Find("DiagonalDown");

  }

  void OnEnable() {
    //inputManager.OnMovePerformed += HandleOnMovePerformed;
    //inputManager.OnMoveCanceled += HandleOnMoveCanceled;
  }

  void OnDisable() {
    //inputManager.OnMovePerformed -= HandleOnMovePerformed;
    //inputManager.OnMoveCanceled -= HandleOnMoveCanceled;
  }

  //public void ResetMovementParameters() {
  //  animator.SetBool(AnimatorIsCrouching, false);
  //  animator.SetBool(AnimatorIsCrouchingLoop, false);
  //  animator.SetBool(AnimatorIsJumping, false);
  //  animator.SetBool(AnimatorIsAiming, false);
  //}

  //public void ResetActionParameters() {
  //  animator.SetBool(AnimatorIsShooting, false);
  //  //animator.SetBool(AnimatorIsTakingDamage, false);
  //  //animator.SetBool(AnimatorIsShootingEX, false);
  //}

  //public void SetParameterIsShooting() {
  //  animator.SetBool(AnimatorIsShooting, true);
  //}

  //public void SetParameterIsMoving() {
  //  animator.SetBool(AnimatorIsMoving, true);
  //}

  //public void SetParameterIsJumping() {
  //  animator.SetBool(AnimatorIsJumping, true);
  //}

  //public void SetParameterIsCrouching() {
  //  animator.SetBool(AnimatorIsCrouching, true);
  //}

  //public void SetParameterIsDashing() {
  //  animator.SetBool(AnimatorIsDashing, true);
  //}

  //public void SetParameterIsAiming() {
  //  animator.SetBool(AnimatorIsAiming, true);
  //}
  //public void SetParameterIsAimingDirection() {
  //  //animator.SetBool(AnimatorIsAimingDirection, true);
  //}

  //void HandleOnMovePerformed(Vector2 vector) {
  //  animator.SetFloat(AnimatorXVelocity, vector.x);
  //  animator.SetFloat(AnimatorYVelocity, vector.y);
  //  animator.SetInteger(AnimatorXAim, Mathf.RoundToInt(vector.x));
  //  animator.SetInteger(AnimatorYAim, Mathf.RoundToInt(vector.y));
  //  //animator.SetBool(AnimatorIsAimingDirection, true);
  //  //if (vector.x > 0) {
  //  //  animator.SetBool(AnimatorIsMoving, true);
  //  //}
  //}
  //void HandleOnMoveCanceled() {
  //  animator.SetFloat(AnimatorXVelocity, 0f);
  //  animator.SetFloat(AnimatorYVelocity, 0f);
  //  animator.SetInteger(AnimatorXAim, 0);
  //  animator.SetInteger(AnimatorYAim, 0);
  //  animator.SetBool(AnimatorIsMoving, false);
  //  //animator.SetBool(AnimatorIsAimingDirection, false);
  //}

  //public void OnCrouchingEntryEnd() {
  //  animator.SetBool(AnimatorIsCrouching, false);
  //  animator.SetBool(AnimatorIsCrouchingLoop, true);
  //}
}
