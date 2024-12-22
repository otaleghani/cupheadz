using UnityEngine;

public class TestParryObject : MonoBehaviour, IParryable {
  private ParrySparksManager parrySparksManager;
  private int counter = 0;

  private void Awake() {
    parrySparksManager = GameObject.Find("ParrySparksManager").GetComponent<ParrySparksManager>();
  }

  public void OnParry() {
    parrySparksManager.ShowSpark(gameObject.transform);
    gameObject.SetActive(false);
  }

  private void FixedUpdate() {
    counter += 1;
    if (counter > 50) {
      gameObject.SetActive(true);
    }
  }
}
