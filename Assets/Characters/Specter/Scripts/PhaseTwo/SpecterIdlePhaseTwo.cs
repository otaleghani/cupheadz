using UnityEngine;

public class SpecterIdlePhaseTwo : MonoBehaviour, IBossAction
{
  public void Enter()
  {
    SpecterStateManager.Instance.ChangeAnimation(("Phase2__Idle"));
  }

  public void Update() { }
  public void Exit() { }
}
