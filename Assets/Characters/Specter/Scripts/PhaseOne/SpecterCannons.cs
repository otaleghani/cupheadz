public class SpecterCannons : IBossAction {
  public void Enter() {
    SpecterStateManager.Instance.ChangeAnimation("Phase1__CannonTransform"); 
  }
  
  public void Update() { }
  
  public void Exit() { }
}
