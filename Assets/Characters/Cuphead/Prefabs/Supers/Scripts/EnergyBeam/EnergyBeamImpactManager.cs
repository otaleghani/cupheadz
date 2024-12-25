using UnityEngine;

public class EnergyBeamImpact : MonoBehaviour {
  public void OnAnimationEnd() {
    Destroy(gameObject);
  }
}
