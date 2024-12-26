using UnityEngine;

public class GiantGhostSuper : MonoBehaviour, ISuperAttack {
  private Transform giantGhostSpawn;
  private CupheadSuperManager superManager;
  private GameObject giantGhostPrefab = null;
  private float counter;

  private void Awake() {
    giantGhostSpawn = transform.parent.Find("GiantGhostSpawner");
    superManager = GetComponentInParent<CupheadSuperManager>();
    giantGhostPrefab = Resources.Load<GameObject>("Super__GiantGhost");
  }

  public void UseSuper() {
    GameObject giantGhost = Instantiate(giantGhostPrefab, BulletPoolManager.Instance.transform);
    giantGhost.transform.position = giantGhostSpawn.position;
  }
}
