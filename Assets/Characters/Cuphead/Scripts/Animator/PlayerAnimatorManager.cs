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
  public event Action OnExShootingAnimationMidPoint;
  public event Action OnExShootingAnimationEnd;
  public event Action OnParryAnimationEnd;
  public event Action OnSuperAnimationEnd;
  public event Action OnDamageAnimationEnd;
  public event Action OnDeathAnimationEnd;
  public event Action OnIntroAnimationEnd;

  public enum PlayerAnimations {
    None,
    Intro,
    Death,
    Damage,
    Idle,
    Running,
    RunningShooting,
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
    ParryingPink,
    ShootingExGroundUp,
    ShootingExGroundDown,
    ShootingExGroundFront,
    ShootingExGroundDiagonalUp,
    ShootingExGroundDiagonalDown,
    ShootingExAirUp,
    ShootingExAirDown,
    ShootingExAirFront,
    ShootingExAirDiagonalUp,
    ShootingExAirDiagonalDown,
    SuperEnergyBeam,
    SuperInvincibility,
    SuperGiantGhost,
  }

  private Dictionary<PlayerAnimations, string> animations = 
    new Dictionary<PlayerAnimations, string>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> aimAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> shootAimAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> shootRecoilAnimations = 
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> shootExGroundAnimations =
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();
  public Dictionary<PlayerInputManager.AimDirection, PlayerAnimations> shootExAirAnimations =
    new Dictionary<PlayerInputManager.AimDirection, PlayerAnimations>();

  public void ChangeAnimation(PlayerAnimations animation) {
    animator.Play(animations[animation]);
  }

  void Awake() {
    stateManager = GetComponent<PlayerStateManager>();
    animator = GetComponent<Animator>();

    animations[PlayerAnimations.None] = "None";
    animations[PlayerAnimations.Intro] = "Intro";
    animations[PlayerAnimations.Death] = "Death";
    animations[PlayerAnimations.Damage] = "Damage";
    animations[PlayerAnimations.Idle] = "Idle";
    animations[PlayerAnimations.Running] = "Run";
    animations[PlayerAnimations.RunningShooting] = "RunShoot";
    animations[PlayerAnimations.Jumping] = "Jump";
    animations[PlayerAnimations.Crouching] = "Crouch";
    animations[PlayerAnimations.CrouchingIdle] = "CrouchIdle";
    animations[PlayerAnimations.CrouchingExit] = "CrouchExit";
    animations[PlayerAnimations.CrouchingShootingAim] = "CrouchShootAim";
    animations[PlayerAnimations.CrouchingShootingRecoil] = "CrouchShootRecoil";
    animations[PlayerAnimations.Dashing] = "Dash";
    animations[PlayerAnimations.AimingFront] = "AimFront";
    animations[PlayerAnimations.AimingUp] = "AimUp";
    animations[PlayerAnimations.AimingDown] = "AimDown";
    animations[PlayerAnimations.AimingDiagonalUp] = "AimDiagonalUp";
    animations[PlayerAnimations.AimingDiagonalDown] = "AimDiagonalDown";

    animations[PlayerAnimations.ShootingRecoilFront] = "ShootRecoilFront";
    animations[PlayerAnimations.ShootingRecoilUp] = "ShootRecoilUp";
    animations[PlayerAnimations.ShootingRecoilDown] = "ShootRecoilDown";
    animations[PlayerAnimations.ShootingRecoilDiagonalUp] = "ShootRecoilDiagonalUp";
    animations[PlayerAnimations.ShootingRecoilDiagonalDown] = "ShootRecoilDiagonalDown";

    animations[PlayerAnimations.ShootingAimFront] = "ShootAimFront";
    animations[PlayerAnimations.ShootingAimUp] = "ShootAimUp";
    animations[PlayerAnimations.ShootingAimDown] = "ShootAimDown";
    animations[PlayerAnimations.ShootingAimDiagonalUp] = "ShootAimDiagonalUp";
    animations[PlayerAnimations.ShootingAimDiagonalDown] = "ShootAimDiagonalDown";

    animations[PlayerAnimations.Parrying] = "Parry";
    animations[PlayerAnimations.ParryingPink] = "ParryPink";

    animations[PlayerAnimations.ShootingExGroundUp] = "ShootExGroundUp";
    animations[PlayerAnimations.ShootingExGroundDown] = "ShootExGroundDown";
    animations[PlayerAnimations.ShootingExGroundFront] = "ShootExGroundFront";
    animations[PlayerAnimations.ShootingExGroundDiagonalUp] = "ShootExGroundDiagonalUp";
    animations[PlayerAnimations.ShootingExGroundDiagonalDown] = "ShootExGroundDiagonalDown";
    animations[PlayerAnimations.ShootingExAirUp] = "ShootExAirUp";
    animations[PlayerAnimations.ShootingExAirDown] = "ShootExAirDown";
    animations[PlayerAnimations.ShootingExAirFront] = "ShootExAirFront";
    animations[PlayerAnimations.ShootingExAirDiagonalUp] = "ShootExAirDiagonalUp";
    animations[PlayerAnimations.ShootingExAirDiagonalDown] = "ShootExAirDiagonalDown";

    animations[PlayerAnimations.SuperEnergyBeam] = "SuperEnergyBeam";
    animations[PlayerAnimations.SuperInvincibility] = "SuperInvincibility";
    animations[PlayerAnimations.SuperGiantGhost] = "SuperGiantGhost";

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

    shootExGroundAnimations[PlayerInputManager.AimDirection.Front] = 
      PlayerAnimations.ShootingExGroundFront;
    shootExGroundAnimations[PlayerInputManager.AimDirection.Up] = 
      PlayerAnimations.ShootingExGroundUp;
    shootExGroundAnimations[PlayerInputManager.AimDirection.Down] = 
      PlayerAnimations.ShootingExGroundDown;
    shootExGroundAnimations[PlayerInputManager.AimDirection.DiagonalUp] = 
      PlayerAnimations.ShootingExGroundDiagonalUp;
    shootExGroundAnimations[PlayerInputManager.AimDirection.DiagonalDown] = 
      PlayerAnimations.ShootingExGroundDiagonalDown;

    shootExAirAnimations[PlayerInputManager.AimDirection.Front] = 
      PlayerAnimations.ShootingExAirFront;
    shootExAirAnimations[PlayerInputManager.AimDirection.Up] = 
      PlayerAnimations.ShootingExAirUp;
    shootExAirAnimations[PlayerInputManager.AimDirection.Down] = 
      PlayerAnimations.ShootingExAirDown;
    shootExAirAnimations[PlayerInputManager.AimDirection.DiagonalUp] = 
      PlayerAnimations.ShootingExAirDiagonalUp;
    shootExAirAnimations[PlayerInputManager.AimDirection.DiagonalDown] = 
      PlayerAnimations.ShootingExAirDiagonalDown;
  }

  // Helper function used to Pause and Resume an animation
  public void Pause() {
    animator.speed = 0;
  }
  public void Resume() {
    animator.speed = 1;
  }

  // Helper function used to calculate the current frame of an animation
  public int GetAnimationCurrentFrame() {
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    float currentTime = stateInfo.normalizedTime * stateInfo.length;
    // 24 beign the framerate
    int currentFrame = Mathf.FloorToInt(currentTime * 24) % Mathf.FloorToInt(stateInfo.length * 24);
    return currentFrame;
  }

  // Helper function to start an animation from a specific frame 
  public void ChangeAnimationFromFrame(PlayerAnimations animation, int frame) {
    float normalizedTime = frame / 24;
    animator.Play(animations[animation], 0, normalizedTime);
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

  /// <summary>
  /// Whenever one of the ExShooting animation arrives at the point of spawning the bullet
  /// we use this function to invoke an event so that the WeaponManager can call ExShoot()
  /// </summary>
  public void OnExShootingMidPoint() {
    OnExShootingAnimationMidPoint?.Invoke();
  }
  public void OnExShootingEnd() {
    OnExShootingAnimationEnd?.Invoke();
  }
  public void OnSuperEnd() {
    OnSuperAnimationEnd?.Invoke();
  }
  public void OnParryEnd() {
    OnParryAnimationEnd?.Invoke();
  }
  public void OnDamageEnd() {
    OnDamageAnimationEnd?.Invoke();
  }
  public void OnDeathEnd() {
    OnDeathAnimationEnd?.Invoke();
  }

  public void OnIntroEnd() {
    OnIntroAnimationEnd?.Invoke();
  }
}
