using UnityEngine;
using System.Collections;

public class ObjectResetter : MonoBehaviour {
  public static ObjectResetter instance;

  private void Awake() {
    if (instance == null) {
      instance = this;
    } else {
      Debug.LogWarning("More than one ObjectResetter fount in scene");
    }
  }

  public void ResetAfter(GameObject obj, int seconds) {
    StartCoroutine(ResetObjectAfter(obj, seconds));
  }

  private IEnumerator ResetObjectAfter(GameObject obj, int seconds) {
    obj.SetActive(false);
    yield return new WaitForSeconds(seconds);
    obj.SetActive(true);
  }
}
