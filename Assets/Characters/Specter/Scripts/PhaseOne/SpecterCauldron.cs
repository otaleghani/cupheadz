public class SpecterCauldron : IBossAction {
  public void Enter() {
    SpecterStateManager.Instance.ChangeAnimation("Phase1__PortalIn");
    SpecterStateManager.Instance.DisableCollider();
  }
  public void Update() { }
  
  public void Exit() {
    SpecterStateManager.Instance.EnableCollider();
  }
}
