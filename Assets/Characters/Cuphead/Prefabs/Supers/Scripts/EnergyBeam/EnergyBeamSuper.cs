using UnityEngine;

public class EnergyBeamSuper : MonoBehaviour, ISuperAttack {
  private Transform energyBeamSfxSpawn;
  private Transform energyBeamSpawn;
  private CupheadSuperManager superManager;

  // Here I should handle the Pool

  private void Awake() {
    superManager = GetComponentInParent<CupheadSuperManager>();
    energyBeamSfxSpawn = transform.parent.Find("EnergyBeamSfxSpawner");
    energyBeamSpawn = transform.parent.Find("EnergyBeamSpawner");
  }
  
  public void UseSuper() {
    if (superManager.movementManager.isFacingRight) {
      GameObject EnergyBeamSfx = Instantiate(Resources.Load<GameObject>("Super__EnergyBeamSfx"), BulletPoolManager.Instance.transform);
      EnergyBeamSfx.transform.position = energyBeamSfxSpawn.position;

      GameObject EnergyBeam = Instantiate(Resources.Load<GameObject>("Super__EnergyBeam"), BulletPoolManager.Instance.transform);
      EnergyBeam.transform.position = energyBeamSpawn.position;
    } else {
      GameObject EnergyBeamSfx = Instantiate(Resources.Load<GameObject>("Super__EnergyBeamSfx"), BulletPoolManager.Instance.transform);
      EnergyBeamSfx.transform.position = energyBeamSfxSpawn.position;
      Vector3 ls = EnergyBeamSfx.transform.localScale;
      ls.x *= -1f;
      EnergyBeamSfx.transform.localScale = ls;

      GameObject EnergyBeam = Instantiate(Resources.Load<GameObject>("Super__EnergyBeam"), BulletPoolManager.Instance.transform);
      EnergyBeam.transform.position = energyBeamSpawn.position;
      ls = EnergyBeam.transform.localScale;
      ls.x *= -1f;
      EnergyBeam.transform.localScale = ls;
    }
  }

}
