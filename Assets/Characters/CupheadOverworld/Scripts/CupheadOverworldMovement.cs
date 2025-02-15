using UnityEngine;

public class CupheadOverworldMovement : MonoBehaviour {
  private PlayerInputManager inputManager;
  private CupheadOverwolrdAnimatorManager animatorManager;
  private GameObject tooltip;
  private GameObject interactableObject;
  private Rigidbody2D rb;
  private float speed = 3f;
  private Vector2 movement;
  private bool isFacingRight = true;
  private int x;
  private int y;

  private void Awake() {
    inputManager = GetComponent<PlayerInputManager>();
    animatorManager = GetComponent<CupheadOverwolrdAnimatorManager>();
    tooltip = transform.Find("InteractTooltip").gameObject;
    rb = GetComponent<Rigidbody2D>();
  }
  private void OnEnable() {
    inputManager.OnMovePerformed += HandleMove;
    inputManager.OnMoveCanceled += HandleMoveCanceled;
    inputManager.OnJumpPerformed += HandleInteract;
  }
  private void OnDisable() {
    inputManager.OnMovePerformed -= HandleMove;
    inputManager.OnMoveCanceled -= HandleMoveCanceled;
    inputManager.OnJumpPerformed -= HandleInteract;
  }

  private void FixedUpdate() {
    rb.linearVelocity = new Vector2(
      movement.x * speed,
      movement.y * speed
    );
    HandleFlipCharacter();
  }

  private void HandleMove(Vector2 dir) {
    x = inputManager.xPosition;
    y = inputManager.yPosition;
    movement = dir;
    animatorManager.lastMovementAnimation = animatorManager.moveAnimations[(x, y)];
    animatorManager.PlayAnimation(animatorManager.lastMovementAnimation);
    Debug.Log(animatorManager.lastMovementAnimation);
  }

  private void HandleMoveCanceled() {
    animatorManager.PlayAnimation(animatorManager.standAnimations[animatorManager.lastMovementAnimation]);
    movement = new Vector2(0, 0);
  }
  void HandleFlipCharacter() {
    if (isFacingRight && inputManager.xPosition < 0 ||
    !isFacingRight && inputManager.xPosition > 0) {
      isFacingRight = !isFacingRight;
      Vector3 ls = transform.localScale;
      ls.x *= -1f;
      transform.localScale = ls;
    }
  }

  private void OnTriggerEnter2D(Collider2D other) { 
    Debug.Log("Enter!!!");
    if (other.gameObject.CompareTag("MapInteractable")) {
      tooltip.SetActive(true);
      interactableObject = other.gameObject;
    }
  }
  private void OnTriggerExit2D(Collider2D other) { 
    Debug.Log("Exit!!!");
    if (other.gameObject.CompareTag("MapInteractable")) {
      tooltip.SetActive(false);
      interactableObject = null;
    }
  }
  private void HandleInteract() {
    if (interactableObject != null) {
      interactableObject.GetComponent<MapInteractableObjectManager>().HandleInteract();
    }
  }
}
