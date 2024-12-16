using UnityEngine;
using System.Collections.Generic;

public class PlayerAnimatorManager : MonoBehaviour {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private Animator animator;

  public enum PlayerAnimations {
    Idle,
    Running,
    RunningShootingAim,
    RunningShootingRecoil,
    Jumping,
    Crouching,
    CrouchingLoop,
    CrouchingShootingAim,
    CrouchingShootingRecoil,
    Dashing,
    AimingFront,
    AimingUp,
    AimingDown,
    AimingDiagonalUp,
    AimingDiagonalDown,
    ShootingRecoilFront,
    ShootingRecoilUp,
    ShootingRecoilDown,
    ShootingRecoilDiagonalUp,
    ShootingRecoilDiagonalDown,
    ShootingAimFront,
    ShootingAimUp,
    ShootingAimDown,
    ShootingAimDiagonalUp,
    ShootingAimDiagonalDown,
    Parrying,
  }

  private Dictionary<PlayerAnimations, string> animations = new Dictionary<PlayerAnimations, string>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> aimAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> shootAimAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> shootRecoilAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();

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

    animations[PlayerAnimations.Idle] = "Cuphead__Idle";
    animations[PlayerAnimations.Running] = "Cuphead__Running";
    animations[PlayerAnimations.RunningShootingAim] = "Cuphead__RunningShootingAim";
    animations[PlayerAnimations.RunningShootingRecoil] = "Cuphead__RunningShootingRecoil";
    animations[PlayerAnimations.Jumping] = "Cuphead__Jumping";
    animations[PlayerAnimations.Crouching] = "Cuphead__Crouching";
    animations[PlayerAnimations.CrouchingLoop] = "Cuphead__CrouchingLoop";
    animations[PlayerAnimations.CrouchingShootingAim] = "Cuphead__CrouchingShootingAim";
    animations[PlayerAnimations.CrouchingShootingRecoil] = "Cuphead__CrouchingShootingRecoil";
    animations[PlayerAnimations.Dashing] = "Cuphead__Dashing";
    animations[PlayerAnimations.AimingFront] = "Cuphead__AimingFront";
    animations[PlayerAnimations.AimingUp] = "Cuphead__AimingUp";
    animations[PlayerAnimations.AimingDown] = "Cuphead__AimingDown";
    animations[PlayerAnimations.AimingDiagonalUp] = "Cuphead__AimingDiagonalUp";
    animations[PlayerAnimations.AimingDiagonalDown] = "Cuphead__AimingDiagonalDown";

    animations[PlayerAnimations.ShootingRecoilFront] = "Cuphead__ShootingRecoilFront";
    animations[PlayerAnimations.ShootingRecoilUp] = "Cuphead__ShootingRecoilUp";
    animations[PlayerAnimations.ShootingRecoilDown] = "Cuphead__ShootingRecoilDown";
    animations[PlayerAnimations.ShootingRecoilDiagonalUp] = "Cuphead__ShootingRecoilDiagonalUp";
    animations[PlayerAnimations.ShootingRecoilDiagonalDown] = "Cuphead__ShootingRecoilDiagonalDown";

    animations[PlayerAnimations.ShootingAimFront] = "Cuphead__ShootingAimFront";
    animations[PlayerAnimations.ShootingAimUp] = "Cuphead__ShootingAimUp";
    animations[PlayerAnimations.ShootingAimDown] = "Cuphead__ShootingAimDown";
    animations[PlayerAnimations.ShootingAimDiagonalUp] = "Cuphead__ShootinAimgDiagonalUp";
    animations[PlayerAnimations.ShootingAimDiagonalDown] = "Cuphead__ShootingAimDiagonalDown";

    animations[PlayerAnimations.Parrying] = "Cuphead__Parrying";

    aimAnimations[PlayerInputManager.AimDirection.Front] = PlayerAnimations.AimingFront;
    aimAnimations[PlayerInputManager.AimDirection.Up] = PlayerAnimations.AimingUp;
    aimAnimations[PlayerInputManager.AimDirection.Down] = PlayerAnimations.AimingDown;
    aimAnimations[PlayerInputManager.AimDirection.DiagonalUp] = PlayerAnimations.AimingDiagonalUp;
    aimAnimations[PlayerInputManager.AimDirection.DiagonalDown] = PlayerAnimations.AimingDiagonalDown;

    shootAimAnimations[PlayerInputManager.AimDirection.Front] = 
      PlayerAnimations.ShootingAimFront;
    shootAimAnimations[PlayerInputManager.AimDirection.Up] = 
      PlayerAnimations.ShootingAimUp;
    shootAimAnimations[PlayerInputManager.AimDirection.Down] = 
      PlayerAnimations.ShootingAimDown;
    shootAimAnimations[PlayerInputManager.AimDirection.DiagonalUp] = 
      PlayerAnimations.ShootingAimDiagonalUp;
    shootAimAnimations[PlayerInputManager.AimDirection.DiagonalDown] = 
      PlayerAnimations.ShootingAimDiagonalDown;

    shootRecoilAnimations[PlayerInputManager.AimDirection.Front] = 
      PlayerAnimations.ShootingRecoilFront;
    shootRecoilAnimations[PlayerInputManager.AimDirection.Up] = 
      PlayerAnimations.ShootingRecoilUp;
    shootRecoilAnimations[PlayerInputManager.AimDirection.Down] = 
      PlayerAnimations.ShootingRecoilDown;
    shootRecoilAnimations[PlayerInputManager.AimDirection.DiagonalUp] = 
      PlayerAnimations.ShootingRecoilDiagonalUp;
    shootRecoilAnimations[PlayerInputManager.AimDirection.DiagonalDown] = 
      PlayerAnimations.ShootingRecoilDiagonalDown;
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
