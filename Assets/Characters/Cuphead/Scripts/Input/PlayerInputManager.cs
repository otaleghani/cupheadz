using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Handles the player input with event Actions. A script can subscribe to 
/// one or more events to handle actions.
/// </summary>
public class PlayerInputManager : MonoBehaviour {
  public event Action<Vector2> OnMovePerformed;
  public event Action OnMoveCanceled;
  public event Action<Vector2> OnMoveY;
  public event Action OnMoveYCanceled;
  public event Action OnJump;
  public event Action OnJumpReleased;
  public event Action OnShoot;
  public event Action OnShootReleased;
  public event Action OnCrouch;
  public event Action OnCrouchReleased;
  public event Action OnLock;
  public event Action OnLockReleased;
  public event Action OnDash;

  private PlayerInput playerInput;
  private InputAction moveAction;
  private InputAction jumpAction;
  private InputAction shootAction;
  private InputAction lockAction;
  private InputAction dashAction;
  private InputAction crouchAction;
  //private InputAction shootEXAction;
  //private InputAction switchWeaponAction;

  void Awake() {
    playerInput = GetComponent<PlayerInput>();

    moveAction = playerInput.actions["Move"];
    jumpAction = playerInput.actions["Jump"];
    shootAction = playerInput.actions["Shoot"];
    lockAction = playerInput.actions["Lock"];
    dashAction = playerInput.actions["Dash"];
    crouchAction = playerInput.actions["Crouch"];
    //shootEXAction = playerInput.actions["ShootEX"];
    //switchWeaponAction = playerInput.actions["SwitchWeapon"];
  }

  void OnEnable() {
    moveAction.performed += OnMoveActionPerformed;
    moveAction.canceled += OnMoveActionCanceled;
  }

  void OnDisable() {
    moveAction.performed -= OnMoveActionPerformed;
    moveAction.canceled -= OnMoveActionCanceled;
  }

  private void OnMoveActionPerformed(InputAction.CallbackContext context) {
    OnMove?.Invoke(context.ReadValue<Vector2>());
  }
  
  private void OnMoveActionCanceled(InputAction.CallbackContext context) {
    OnMoveCanceled.Invoke();
  }

  private bool resetMoveCanceled = true;
  void FixedUpdate() {
    // Here I'll need to take into account even the y axis, especially for the y movement
    if (moveAction.ReadValue<Vector2>().x != 0) {
      OnMove?.Invoke(moveAction.ReadValue<Vector2>());
      resetMoveCanceled = true;
    } else {
      if (resetMoveCanceled) {
        OnMoveCanceled?.Invoke();
        resetMoveCanceled = false;
      } 
    }
    if (moveAction.ReadValue<Vector2>().y != 0) {
      OnMoveY?.Invoke(moveAction.ReadValue<Vector2>());
    } else {
      OnMoveYCanceled?.Invoke();
    }
    if (jumpAction.ReadValue<float>() != 0) {
      OnJump?.Invoke();
    } else {
      OnJumpReleased?.Invoke();
    }
    if (shootAction.ReadValue<float>() != 0) {
      OnShoot?.Invoke();
    } else {
      OnShootReleased?.Invoke();
    }
    if (crouchAction.ReadValue<float>() != 0) {
      OnCrouch?.Invoke();
    } else {
      OnCrouchReleased?.Invoke();
    }
    if (lockAction.ReadValue<float>() != 0) {
      OnLock?.Invoke();
    } else {
      OnLockReleased?.Invoke();
    }
    // TODO: Create a minimum cooldown for this state
    if (dashAction.ReadValue<float>() != 0) {
      OnDash?.Invoke();
    }
  }
}
