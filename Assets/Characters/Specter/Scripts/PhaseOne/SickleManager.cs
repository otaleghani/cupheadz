using UnityEngine;
using System.Collections;

public class SickleManager : MonoBehaviour {
  float duration = 2f;

  private void Awake() {
    // 
  }
  
  // Find correct point
  public void ExitScreen() {
    StartCoroutine(Move(GameObject.Find("Specter/SickleExitPoint").transform.position, duration));
  }
  
  public void Move() {
    
  }

  //private IEnumerator MoveManager() { }

  private IEnumerator Move(Vector3 destination, float duration) {
    Vector3 startPos = transform.position;
    float elapsed = 0f;
    while (elapsed < duration) {
      float t = Mathf.Clamp01(elapsed / duration);
      //Vector3 pos = Mathf.Pow(1 - t, 2) * startPos + Mathf.Pow(t, 2) * destination;
      transform.position = Vector3.Lerp(startPos, destination, t);
      elapsed += Time.deltaTime;
      yield return null;
    }

    transform.position = destination;
  }
}
