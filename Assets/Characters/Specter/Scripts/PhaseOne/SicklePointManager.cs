using UnityEngine;
using System.Collections;

public class SicklePointManager : MonoBehaviour {
  private Vector3 origin;
  private float duration;
  private float toAdd;
  
  private void Awake() {
    origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    toAdd = 0.01f;
  }
  
  public void Reset() {
    transform.position = origin;
  }

  private void FixedUpdate() {
    if (duration >= 1) {
      toAdd *= -1;
      duration = 0;
    }
    Vector3 newPos = transform.position;
    newPos.x += toAdd;
    transform.position = newPos;
    duration += Time.deltaTime;
  }
}
