public class SpecterCannons : IBossAction {
  public void Enter() {
    SpecterStateManager.Instance.ChangeAnimation("Phase1__CannonTransform"); 
  }
  
  public void Update() {
    // How do I know when it's time to return to Idle?
  }
  
  public void Exit() { }
}
