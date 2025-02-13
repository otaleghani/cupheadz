using UnityEngine;

public class SpecterClock : IBossAction {
  public void Enter() {
    SpecterStateManager.Instance.ChangeAnimation("Phase1__Clock");
  }

  public void Update() {}

  public void Exit() {}
}
