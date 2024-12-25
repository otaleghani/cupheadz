using UnityEngine;
public class EnergyBeamSfxManager : MonoBehaviour {
  public void OnAnimationEnd() {
    Destroy(gameObject);
  }
}
