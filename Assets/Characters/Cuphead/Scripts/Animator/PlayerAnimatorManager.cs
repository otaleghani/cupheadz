using UnityEngine;
using System.Collections.Generic;

public class PlayerAnimatorManager : MonoBehaviour {
  private PlayerStateManager stateManager;
  private PlayerInputManager inputManager;
  private Animator animator;
  public bool isCrouchingEnter = false;
  public bool isCrouchingExit = false;

  public enum PlayerAnimations {
    Idle,
    Running,
    RunningShootingAim,
    RunningShootingRecoil,
    Jumping,
    Crouching,
    CrouchingIdle,
    CrouchingExit,
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

    animations[PlayerAnimations.Idle] = "Idle";
    animations[PlayerAnimations.Running] = "Running";
    animations[PlayerAnimations.RunningShootingAim] = "RunningShootingAim";
    animations[PlayerAnimations.RunningShootingRecoil] = "RunningShootingRecoil";
    animations[PlayerAnimations.Jumping] = "Jumping";
    animations[PlayerAnimations.Crouching] = "Crouching";
    animations[PlayerAnimations.CrouchingIdle] = "CrouchingIdle";
    animations[PlayerAnimations.CrouchingExit] = "CrouchingExit";
    animations[PlayerAnimations.CrouchingShootingAim] = "CrouchingShootingAim";
    animations[PlayerAnimations.CrouchingShootingRecoil] = "CrouchingShootingRecoil";
    animations[PlayerAnimations.Dashing] = "Dashing";
    animations[PlayerAnimations.AimingFront] = "AimingFront";
    animations[PlayerAnimations.AimingUp] = "AimingUp";
    animations[PlayerAnimations.AimingDown] = "AimingDown";
    animations[PlayerAnimations.AimingDiagonalUp] = "AimingDiagonalUp";
    animations[PlayerAnimations.AimingDiagonalDown] = "AimingDiagonalDown";

    animations[PlayerAnimations.ShootingRecoilFront] = "ShootingRecoilFront";
    animations[PlayerAnimations.ShootingRecoilUp] = "ShootingRecoilUp";
    animations[PlayerAnimations.ShootingRecoilDown] = "ShootingRecoilDown";
    animations[PlayerAnimations.ShootingRecoilDiagonalUp] = "ShootingRecoilDiagonalUp";
    animations[PlayerAnimations.ShootingRecoilDiagonalDown] = "ShootingRecoilDiagonalDown";

    animations[PlayerAnimations.ShootingAimFront] = "ShootingAimFront";
    animations[PlayerAnimations.ShootingAimUp] = "ShootingAimUp";
    animations[PlayerAnimations.ShootingAimDown] = "ShootingAimDown";
    animations[PlayerAnimations.ShootingAimDiagonalUp] = "ShootinAimgDiagonalUp";
    animations[PlayerAnimations.ShootingAimDiagonalDown] = "ShootingAimDiagonalDown";

    animations[PlayerAnimations.Parrying] = "Parrying";

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

  public void OnRecoilAnimationEnd() {
    stateManager.currentShootingState = PlayerStateManager.ShootingState.Aim;
  }

  public void OnCrouchingEntryEnd() {
    ChangeAnimation(PlayerAnimations.CrouchingIdle);
    isCrouchingEnter = false;
  }
  public void OnCrouchingExitEnd() {
    ChangeAnimation(PlayerAnimations.Idle);
    isCrouchingExit = false;
  }
}
