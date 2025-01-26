using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class BootstrapperManager : MonoBehaviour {
  public GameObject Managers;
  
  private void Awake() {
    GameObject obj = Instantiate(Managers);
    obj.name = "Managers";
  }
}
