using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkeletonSquareManager : MonoBehaviour {
  private GameObject p0;
  private GameObject p1;
  private GameObject p2;
  private GameObject p3;
  private GameObject p4;
  private GameObject p5;
  private GameObject p6;
  private GameObject p7;
  // private float movementDurationOne = 0.458f;
  // private float movementDurationTwo = 0.542f;

  private float movementDurationOne = 0.258f;
  private float movementDurationTwo = 0.342f;
  
  private PolygonCollider2D polygonCollider;
  private SpriteRenderer spriteRenderer;
  private List<Vector2> physicsShapePoints = new List<Vector2>();  
  
  private void Awake() {
    p0 = transform.parent.Find("MovePoints/0")?.gameObject;
    p1 = transform.parent.Find("MovePoints/1")?.gameObject;
    p2 = transform.parent.Find("MovePoints/2")?.gameObject;
    p3 = transform.parent.Find("MovePoints/3")?.gameObject;
    p4 = transform.parent.Find("MovePoints/4")?.gameObject;
    p5 = transform.parent.Find("MovePoints/5")?.gameObject;
    p6 = transform.parent.Find("MovePoints/6")?.gameObject;
    p7 = transform.parent.Find("MovePoints/7")?.gameObject;

    polygonCollider = GetComponent<PolygonCollider2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void PlaySound() {
    AudioManager.Instance.Play("Specter__Block__Stone");
    AudioManager.Instance.Play("Specter__Block__OOF");
  }
  
  void LateUpdate() {
    // Get the current sprite from the SpriteRenderer.
    Sprite currentSprite = spriteRenderer.sprite;
    if (currentSprite != null) {
      // Get how many physics shapes the sprite has.
      int shapeCount = currentSprite.GetPhysicsShapeCount();
      // Set the collider's path count to match.
      polygonCollider.pathCount = shapeCount;

      // For each shape, retrieve its points and assign to the collider.
      for (int i = 0; i < shapeCount; i++) {
        // Clear and refill the list with the current shape's points.
        physicsShapePoints.Clear();
        currentSprite.GetPhysicsShape(i, physicsShapePoints);
        // Assign these points as a new path in the collider.
        polygonCollider.SetPath(i, physicsShapePoints.ToArray());
      }
    }
  }
  
  private void OnEnable() {
    transform.position = p0.transform.position;
  }
  
  public void GoP0() {
    StartCoroutine(Move(p0, movementDurationOne));
    // StartCoroutine()
  }

  public void GoP1() {
    StartCoroutine(Move(p1, movementDurationOne));
  }

  public void GoP2() {
    StartCoroutine(Move(p2, movementDurationTwo));
  }
  
  public void GoP3() {
    StartCoroutine(Move(p3, movementDurationTwo));
  }
  
  public void GoP4() {
    StartCoroutine(Move(p4, movementDurationOne));
  }
  
  public void GoP5() {
    StartCoroutine(Move(p5, movementDurationOne));
  }
  
  public void GoP6() {
    StartCoroutine(Move(p6, movementDurationOne));
  }
  
  public void GoP7() {
    StartCoroutine(Move(p7, movementDurationOne));
  }

  private IEnumerator Move(GameObject destination, float duration) {
    Vector3 startPos = transform.position;
    float elapsed = 0f;

    while (elapsed < duration) {
      float t = elapsed / duration;
      Vector3 pos = Vector3.Lerp(startPos, destination.transform.position, t);
      transform.position = pos;
      elapsed += Time.deltaTime;
      yield return null;
    }
    transform.position = destination.transform.position;
    if (destination.name == "7") {
      Destroy(transform.parent.gameObject);
    }
  }
}
