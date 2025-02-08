using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class BootstrapperManager : MonoBehaviour {
  public GameObject[] Managers;
  
  private void Awake() {
    foreach (GameObject m in Managers) {
      GameObject obj = Instantiate(m);
      obj.name = m.name;
    }
  }
}
