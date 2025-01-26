using UnityEngine;
//using System.Collections;

public class SpecterIdlePhaseOne : MonoBehaviour, IBossAction {
  GameObject leftPoint = GameObject.Find("IdleMoveLeft");
  GameObject rightPoint = GameObject.Find("IdleMoveRight");

  public GameObject lastVisited;
  float movementDuration = 1.3f;
  int trips;    
  int maxTrips;
  
  public void Enter() {
    maxTrips = Random.Range(3, 6);
    trips = 0;
    SpecterStateManager.Instance.ChangeAnimation("Phase1__Idle"); 
    SpecterStateManager.OnMoveEnd += OnMoveEnd;
    if (lastVisited == leftPoint) {
      SpecterStateManager.Instance.Move(rightPoint, "arc", movementDuration);
    } else {
      SpecterStateManager.Instance.Move(leftPoint, "arc", movementDuration);
    }
  }
  
  public void Update() {
    // Here I should add a method to move 
  }
  
  public void Exit() {
    SpecterStateManager.OnMoveEnd -= OnMoveEnd;
  }

  private void OnMoveEnd() {
    if (SpecterStateManager.Instance.isFacingRight) {
      SpecterStateManager.Instance.Move(leftPoint, "arc", movementDuration);
      lastVisited = rightPoint;
    } else {
      SpecterStateManager.Instance.Move(rightPoint, "arc", movementDuration);
      lastVisited = leftPoint;
    }
    SpecterStateManager.Instance.HandleFlip();
    trips += 1;
    
    if (trips >= maxTrips) {
      SpecterStateManager.Instance.Attack();
      SpecterStateManager.Instance.Stop();
    }
  }
}
