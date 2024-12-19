using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the player animation using the Animator. It contains all of the possible animations
/// in the enum PlayerAnimations and this gets connected in dictionary "animations" with the
/// name of the animations. Every single animation gets changed by calling the ChangeAnimation
/// method and passing the enum of the desired animation.
/// This class also contains helper methods that gets called at specific frames of the animations
/// and helper Dictionaries to use if you have multiple animations for one action (like Aiming).
/// <summary>
public class PlayerAnimatorManager : MonoBehaviour {
  private PlayerStateManager stateManager;
  private Animator animator;
  public bool isCrouchingEnter = false;
  public bool isCrouchingExit = false;

  public event Action OnDashingAnimationEnd;

  public enum PlayerAnimations {
    Dead,
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

  private Dictionary<PlayerAnimations, string> animations = 
    new Dictionary<PlayerAnimations, string>();
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
    stateManager = GetComponent<PlayerStateManager>();
    animator = GetComponent<Animator>();

    animations[PlayerAnimations.Dead] = "Dead";
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

    aimAnimations[PlayerInputManager.AimDirection.Front] = 
      PlayerAnimations.AimingFront;
    aimAnimations[PlayerInputManager.AimDirection.Up] =
      PlayerAnimations.AimingUp;
    aimAnimations[PlayerInputManager.AimDirection.Down] =
      PlayerAnimations.AimingDown;
    aimAnimations[PlayerInputManager.AimDirection.DiagonalUp] =
      PlayerAnimations.AimingDiagonalUp;
    aimAnimations[PlayerInputManager.AimDirection.DiagonalDown] =
      PlayerAnimations.AimingDiagonalDown;

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

  /// <summary>
  /// Whenever the Recoil animation ends, change the ShootingState to Aim.
  /// Used to cycle throgh the Shooting animation based on the weapon fire rate.
  /// </summary>
  public void OnRecoilAnimationEnd() {
    stateManager.currentShootingState = PlayerStateManager.ShootingState.Aim;
  }

  /// <summary>
  /// Whenever the CrouchingEntry animation ends, change the animation to CrouchingIdle.
  /// </summary>
  public void OnCrouchingEntryEnd() {
    ChangeAnimation(PlayerAnimations.CrouchingIdle);
    isCrouchingEnter = false;
  }

  /// <summary>
  /// Whenever the CrouchingExit animation ends, change the animation to Idle.
  /// </summary>
  public void OnCrouchingExitEnd() {
    ChangeAnimation(PlayerAnimations.Idle);
    isCrouchingExit = false;
  }

  /// <summary>
  /// Whenever the DashingAnimation ends, Invoke the OnDashingAnimationEnd action.
  /// Used to notify the PlayerDashingState that the animation ends, so that it can stop the dash
  /// and change to the next state.
  /// </summary>
  public void OnDashingEnd() {
    OnDashingAnimationEnd?.Invoke();
  }
}
