using System.Collections;
using UnityEngine;

public class DropoffGroundTest : MonoBehaviour, IDropOffGround {
  public void DeactivateCollider(float duration) {
    StartCoroutine(DropOffFor(duration));
  }

  private IEnumerator DropOffFor(float seconds) {
    GetComponent<Collider2D>().isTrigger = true;
    yield return new WaitForSeconds(seconds);
    GetComponent<Collider2D>().isTrigger = false;
  }
}
