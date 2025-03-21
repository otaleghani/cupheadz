using UnityEngine;
//using System.Collections;

public class SpecterIdlePhaseOne : MonoBehaviour, IBossAction
{
  public static SpecterIdlePhaseOne Instance;
  private GameObject leftPoint;
  private GameObject rightPoint;

  public GameObject lastVisited;
  float movementDuration = 1.3f;
  int trips;
  int maxTrips;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Debug.LogWarning("Found more than one instance of SpecterIdlePhaseOne.");
    }
    leftPoint = GameObject.Find("IdleMoveLeft");
    rightPoint = GameObject.Find("IdleMoveRight");
  }

  public void Enter()
  {
    maxTrips = Random.Range(3, 6);
    trips = 0;
    SpecterStateManager.Instance.ChangeAnimation("Phase1__Idle");
    SpecterStateManager.OnMoveEnd += OnMoveEnd;
    if (lastVisited == leftPoint)
    {
      SpecterStateManager.Instance.Move(rightPoint, "arc", movementDuration);
    }
    else
    {
      SpecterStateManager.Instance.Move(leftPoint, "arc", movementDuration);
    }
  }

  public void Update()
  {
    // Here I should add a method to move 
  }

  public void Exit()
  {
    SpecterStateManager.OnMoveEnd -= OnMoveEnd;
  }

  private void OnMoveEnd()
  {
    if (SpecterStateManager.Instance.isFacingRight)
    {
      SpecterStateManager.Instance.Move(leftPoint, "arc", movementDuration);
      lastVisited = rightPoint;
    }
    else
    {
      // Maybe RightPoint??
      SpecterStateManager.Instance.Move(rightPoint, "arc", movementDuration);
      lastVisited = leftPoint;
    }
    SpecterStateManager.Instance.HandleFlip();
    trips += 1;

    if (trips >= maxTrips)
    {
      if (SpecterStateManager.Instance.isPlayerDead)
      {
        return;
      }
      SpecterStateManager.Instance.Attack();
      SpecterStateManager.Instance.Stop();
    }
  }

  private void OnDestroy()
  {
    SpecterStateManager.OnMoveEnd -= OnMoveEnd;
  }
}
