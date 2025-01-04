using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

/// <summary>
/// Handles the player input with event Actions. A script can subscribe to one or more events 
/// to handle actions. This script also handles the current AimDirection of the player and it 
/// also holds the current direction (called xPosition and yPosition) of the movement.
/// </summary>
public class PlayerInputManager : MonoBehaviour {
  public enum AimDirection {
    Up,
    Down,
    Front,
    DiagonalUp,
    DiagonalDown,
  }
  public bool startUi;
  public int xPosition = 0;
  public int yPosition = 0;
  public Vector2 currentVector;
  public static Dictionary<string, PlayerInputManager.AimDirection> coordinates = 
    new Dictionary<string, PlayerInputManager.AimDirection>();
  public static AimDirection CurrentCoordinate = AimDirection.Front;
  public static AimDirection CurrentOppositeCoordinate = AimDirection.Front;

  public event Action<Vector2> OnMovePerformed;
  public event Action OnMoveCanceled;
  public event Action<int, int> OnSerializedMovePerformed;
  public event Action<int, int> OnSerializedMoveCanceled;
  public event Action OnJumpPerformed;
  public event Action OnJumpCanceled;
  public event Action OnShootPerformed;
  public event Action OnShootCanceled;
  public event Action OnCrouchPerformed;
  public event Action OnCrouchCanceled;
  public event Action OnAimPerformed;
  public event Action OnAimCanceled;
  public event Action OnDashPerformed;
  public event Action OnDashCanceled;
  public event Action OnShootEXPerformed;
  public event Action OnShootEXCanceled;
  public event Action OnSwitchWeaponPerformed;
  public event Action OnSwitchWeaponCanceled;
  public event Action OnPausePerformed;
  //public event Action OnPauseCanceled;
  public event Action OnUICancel;

  private PlayerInput playerInput;
  private InputAction moveAction;
  private InputAction jumpAction;
  private InputAction shootAction;
  private InputAction aimAction;
  private InputAction aimDirectionAction;
  private InputAction dashAction;
  private InputAction crouchAction;
  private InputAction shootEXAction;
  private InputAction switchWeaponAction;
  private InputAction pauseAction;
  private InputAction closeAction;
  private InputAction cancelAction;

  void Awake() {
    playerInput = GetComponent<PlayerInput>();
    moveAction = playerInput.actions["Move"];
    jumpAction = playerInput.actions["Jump"];
    shootAction = playerInput.actions["Shoot"];
    aimAction = playerInput.actions["Aim"];
    dashAction = playerInput.actions["Dash"];
    crouchAction = playerInput.actions["Crouch"];
    shootEXAction = playerInput.actions["ShootEX"];
    switchWeaponAction = playerInput.actions["SwitchWeapon"];
    pauseAction = playerInput.actions["Pause"];

    closeAction = playerInput.actions["Close"];
    cancelAction = playerInput.actions["Cancel"];

    coordinates["0,1"] = PlayerInputManager.AimDirection.Up;
    coordinates["0,-1"] = PlayerInputManager.AimDirection.Down;
    coordinates["1,0"] = PlayerInputManager.AimDirection.Front;
    coordinates["0,0"] = PlayerInputManager.AimDirection.Front;
    coordinates["-1,0"] = PlayerInputManager.AimDirection.Front;
    coordinates["1,1"] = PlayerInputManager.AimDirection.DiagonalUp;
    coordinates["-1,1"] = PlayerInputManager.AimDirection.DiagonalUp;
    coordinates["1,-1"] = PlayerInputManager.AimDirection.DiagonalDown;
    coordinates["-1,-1"] = PlayerInputManager.AimDirection.DiagonalDown;

    if (startUi) {
      SwitchToUi();
    }
  }

  void OnEnable() {
    moveAction.performed += OnMoveActionPerformed;
    moveAction.canceled += OnMoveActionCanceled;
    jumpAction.performed += OnJumpActionPerformed;
    jumpAction.canceled += OnJumpActionCanceled;
    shootAction.performed += OnShootActionPerformed;
    shootAction.canceled += OnShootActionCanceled;
    aimAction.performed += OnAimActionPerformed;
    aimAction.canceled += OnAimActionCanceled;
    dashAction.performed += OnDashActionPerformed;
    dashAction.canceled += OnDashActionCanceled;
    crouchAction.performed += OnCrouchActionPerformed;
    crouchAction.canceled += OnCrouchActionCanceled;
    shootEXAction.performed += OnShootEXActionPerformed;
    shootEXAction.canceled += OnShootEXActionCanceled;
    switchWeaponAction.performed += OnSwitchWeaponActionPerformed;
    switchWeaponAction.canceled += OnSwitchWeaponActionCanceled;
    pauseAction.performed += OnPauseActionPerformed;
    closeAction.performed += OnPauseActionPerformed;
    cancelAction.performed += OnCancelActionPerformed;

    //if (startUi) {
    //  SwitchToUi();
    //}
  }

  void OnDisable() {
    moveAction.performed -= OnMoveActionPerformed;
    moveAction.canceled -= OnMoveActionCanceled;
    jumpAction.performed -= OnJumpActionPerformed;
    jumpAction.canceled -= OnJumpActionCanceled;
    shootAction.performed -= OnShootActionPerformed;
    shootAction.canceled -= OnShootActionCanceled;
    aimAction.performed -= OnAimActionPerformed;
    aimAction.canceled -= OnAimActionCanceled;
    dashAction.performed -= OnDashActionPerformed;
    dashAction.canceled -= OnDashActionCanceled;
    crouchAction.performed -= OnCrouchActionPerformed;
    crouchAction.canceled -= OnCrouchActionCanceled;
    shootEXAction.performed -= OnShootEXActionPerformed;
    shootEXAction.canceled -= OnShootEXActionCanceled;
    switchWeaponAction.performed -= OnSwitchWeaponActionPerformed;
    switchWeaponAction.canceled -= OnSwitchWeaponActionCanceled;
    pauseAction.performed -= OnPauseActionPerformed;
    closeAction.performed -= OnPauseActionPerformed;
    cancelAction.performed -= OnCancelActionPerformed;
  }

  public void SwitchToUi() {
    playerInput.actions.FindActionMap("Player").Disable();
    playerInput.actions.FindActionMap("UI").Enable();
  }
  public void SwitchToPlayer() {
    playerInput.actions.FindActionMap("Player").Enable();
    playerInput.actions.FindActionMap("UI").Disable();
  }
  public void SwitchActionMap(string map) {
    playerInput.SwitchCurrentActionMap(map);
  }

  private void OnPauseActionPerformed(InputAction.CallbackContext context) {
    OnPausePerformed?.Invoke();
  }

  private void OnMoveActionPerformed(InputAction.CallbackContext context) {
    currentVector = context.ReadValue<Vector2>();
    int x = Mathf.RoundToInt(currentVector.x);
    int y = Mathf.RoundToInt(currentVector.y);
    CurrentCoordinate = coordinates[x + "," + y];
    CurrentOppositeCoordinate = coordinates[-x + "," + -y];
    xPosition = x;
    yPosition = y;
    OnSerializedMovePerformed?.Invoke(x,y);
    OnMovePerformed?.Invoke(currentVector);
  }
  private void OnMoveActionCanceled(InputAction.CallbackContext context) {
    CurrentCoordinate = coordinates["0,0"];
    CurrentOppositeCoordinate = coordinates["0,0"];
    OnSerializedMoveCanceled?.Invoke(0,0);
    xPosition = 0;
    yPosition = 0;
    OnMoveCanceled?.Invoke();
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
    OnShootCanceled?.Invoke();
  }
  private void OnAimActionPerformed(InputAction.CallbackContext context) {
    OnAimPerformed?.Invoke();
  }
  private void OnAimActionCanceled(InputAction.CallbackContext context) {
    OnAimCanceled?.Invoke();
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
  private void OnCancelActionPerformed(InputAction.CallbackContext context) {
    OnUICancel?.Invoke();
  }
}
