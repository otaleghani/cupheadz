using UnityEngine;
public class EnergyBeamSuper : MonoBehaviour, ISuperAttack {

  private Transform energyBeamSfxSpawn;
  private Transform energyBeamSpawn;

  // Here I should handle the Pool

  private void Awake() {
    energyBeamSfxSpawn = transform.parent.Find("EnergyBeamSfxSpawner");
    energyBeamSpawn = transform.parent.Find("EnergyBeamSpawner");
  }
  
  public void UseSuper() {
    GameObject EnergyBeamSfx = Instantiate(Resources.Load<GameObject>("Super__EnergyBeamSfx"), BulletPoolManager.Instance.transform);
    EnergyBeamSfx.transform.position = energyBeamSfxSpawn.position;

    GameObject EnergyBeam = Instantiate(Resources.Load<GameObject>("Super__EnergyBeam"), BulletPoolManager.Instance.transform);
    EnergyBeam.transform.position = energyBeamSpawn.position;
  }

}
