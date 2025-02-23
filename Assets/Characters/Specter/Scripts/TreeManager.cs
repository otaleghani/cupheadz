using UnityEngine;

public class TreeManager : MonoBehaviour {
  public float GruntTime;
  float counter;
  
  private void Start() {
    counter = 0f;
    SpecterAudioManager.Instance.TreeMovement();
  }

  private void FixedUpdate() {
    counter += Time.deltaTime;
    if (counter > GruntTime) {
      SpecterAudioManager.Instance.TreeGrunt();
      counter = 0f;
    }
  }

  public void NoMoreSounds() {
    foreach (AudioSource source in GetComponents<AudioSource>()){
      source.volume = 0f;      
    }
  }
}
