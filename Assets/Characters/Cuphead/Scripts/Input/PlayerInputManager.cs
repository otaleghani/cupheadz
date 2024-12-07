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
  public event Action OnJumpPerformed;
  public event Action OnJumpCanceled;
  public event Action OnShootPerformed;
  public event Action OnShootCanceled;
  public event Action OnCrouchPerformed;
  public event Action OnCrouchCanceled;
  public event Action OnLockPerformed;
  public event Action OnLockCanceled;
  public event Action OnDashPerformed;
  public event Action OnDashCanceled;
  public event Action OnShootEXPerformed;
  public event Action OnShootEXCanceled;
  public event Action OnSwitchWeaponPerformed;
  public event Action OnSwitchWeaponCanceled;

  private PlayerInput playerInput;
  private InputAction moveAction;
  private InputAction jumpAction;
  private InputAction shootAction;
  private InputAction lockAction;
  private InputAction dashAction;
  private InputAction crouchAction;
  private InputAction shootEXAction;
  private InputAction switchWeaponAction;

  void Awake() {
    playerInput = GetComponent<PlayerInput>();
    moveAction = playerInput.actions["Move"];
    jumpAction = playerInput.actions["Jump"];
    shootAction = playerInput.actions["Shoot"];
    lockAction = playerInput.actions["Lock"];
    dashAction = playerInput.actions["Dash"];
    crouchAction = playerInput.actions["Crouch"];
    shootEXAction = playerInput.actions["ShootEX"];
    switchWeaponAction = playerInput.actions["SwitchWeapon"];
  }

  void OnEnable() {
    moveAction.performed += OnMoveActionPerformed;
    moveAction.canceled += OnMoveActionCanceled;
    jumpAction.performed += OnJumpActionPerformed;
    jumpAction.canceled += OnJumpActionCanceled;
    shootAction.performed += OnShootActionPerformed;
    shootAction.canceled += OnShootActionCanceled;
    lockAction.performed += OnLockActionPerformed;
    lockAction.canceled += OnLockActionCanceled;
    dashAction.performed += OnDashActionPerformed;
    dashAction.canceled += OnDashActionCanceled;
    crouchAction.performed += OnCrouchActionPerformed;
    crouchAction.canceled += OnCrouchActionCanceled;
    shootEXAction.performed += OnShootEXActionPerformed;
    shootEXAction.canceled += OnShootEXActionCanceled;
    switchWeaponAction.performed += OnSwitchWeaponActionPerformed;
    switchWeaponAction.canceled += OnSwitchWeaponActionCanceled;
  }

  void OnDisable() {
    moveAction.performed -= OnMoveActionPerformed;
    moveAction.canceled -= OnMoveActionCanceled;
    jumpAction.performed -= OnJumpActionPerformed;
    jumpAction.canceled -= OnJumpActionCanceled;
    shootAction.performed -= OnShootActionPerformed;
    shootAction.canceled -= OnShootActionCanceled;
    lockAction.performed -= OnLockActionPerformed;
    lockAction.canceled -= OnLockActionCanceled;
    dashAction.performed -= OnDashActionPerformed;
    dashAction.canceled -= OnDashActionCanceled;
    crouchAction.performed -= OnCrouchActionPerformed;
    crouchAction.canceled -= OnCrouchActionCanceled;
    shootEXAction.performed -= OnShootEXActionPerformed;
    shootEXAction.canceled -= OnShootEXActionCanceled;
    switchWeaponAction.performed -= OnSwitchWeaponActionPerformed;
    switchWeaponAction.canceled -= OnSwitchWeaponActionCanceled;
  }

  private void OnMoveActionPerformed(InputAction.CallbackContext context) {
    OnMovePerformed?.Invoke(context.ReadValue<Vector2>());
  }
  private void OnMoveActionCanceled(InputAction.CallbackContext context) {
    OnMoveCanceled.Invoke();
  }
  private void OnJumpActionPerformed(InputAction.CallbackContext context) {
    OnJumpPerformed?.Invoke();
  }
  private void OnJumpActionCanceled(InputAction.CallbackContext context) {
    OnJumpCanceled?.Invoke();
  }
  private void OnShootActionPerformed(InputAction.CallbackContext context) {
    OnShootPerformed?.Invoke();
  }
  private void OnShootActionCanceled(InputAction.CallbackContext context) {
    OnShootCanceled.Invoke();
  }
  private void OnLockActionPerformed(InputAction.CallbackContext context) {
    OnLockPerformed?.Invoke();
  }
  private void OnLockActionCanceled(InputAction.CallbackContext context) {
    OnLockCanceled?.Invoke();
  }
  private void OnDashActionPerformed(InputAction.CallbackContext context) {
    OnDashPerformed?.Invoke();
  }
  private void OnDashActionCanceled(InputAction.CallbackContext context) {
    OnDashCanceled?.Invoke();
  }
  private void OnCrouchActionPerformed(InputAction.CallbackContext context) {
    OnCrouchPerformed?.Invoke();
  }
  private void OnCrouchActionCanceled(InputAction.CallbackContext context) {
    OnCrouchCanceled?.Invoke();
  }
  private void OnShootEXActionPerformed(InputAction.CallbackContext context) {
    OnShootEXPerformed?.Invoke();
  }
  private void OnShootEXActionCanceled(InputAction.CallbackContext context) {
    OnShootEXCanceled?.Invoke();
  }
  private void OnSwitchWeaponActionPerformed(InputAction.CallbackContext context) {
    OnSwitchWeaponPerformed?.Invoke();
  }
  private void OnSwitchWeaponActionCanceled(InputAction.CallbackContext context) {
    OnSwitchWeaponCanceled?.Invoke();
  }
}
