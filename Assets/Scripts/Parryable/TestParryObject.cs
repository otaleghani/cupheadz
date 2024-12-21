using UnityEngine;
public class TestParryObject : MonoBehaviour, IParryable {
  public void OnParry() {
    Destroy(gameObject);
  }
}
