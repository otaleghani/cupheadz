public class SpecterCauldron : IBossAction {
  public void Enter() {
    SpecterStateManager.Instance.ChangeAnimation("Phase1__PortalIn");
  }
  public void Update() { }
  
  public void Exit() { }
}
