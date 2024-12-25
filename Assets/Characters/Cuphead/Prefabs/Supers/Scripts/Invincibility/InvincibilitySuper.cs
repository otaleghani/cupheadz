using UnityEngine;
using System.Collections;

public class InvincibilitySuper : MonoBehaviour, ISuperAttack {
  private Transform invincibilitySpawn;
  private CupheadSuperManager superManager;

  private void Awake() {
    invincibilitySpawn = transform.parent.Find("InvincibilitySpawner");
    superManager = GetComponentInParent<CupheadSuperManager>();
  }

  public void UseSuper() {
    StartCoroutine(InvincibilityBuffering());
    //superManager.stateManager.EnterInvincibility();
    if (superManager.movementManager.isFacingRight) {
      GameObject InvincibilitySfx = Instantiate(Resources.Load<GameObject>("Super__Invincibility"), BulletPoolManager.Instance.transform);
      InvincibilitySfx.transform.position = invincibilitySpawn.position;
    } else {
      GameObject InvincibilitySfx = Instantiate(Resources.Load<GameObject>("Super__Invincibility"), BulletPoolManager.Instance.transform);
      Vector3 ls = InvincibilitySfx.transform.localScale;
      ls.x *= -1f;
      InvincibilitySfx.transform.localScale = ls;
      InvincibilitySfx.transform.position = invincibilitySpawn.position;
    }
  }

  private IEnumerator InvincibilityBuffering() {
    yield return new WaitForSeconds(1.3f);
    superManager.stateManager.EnterInvincibility();
  }
}
