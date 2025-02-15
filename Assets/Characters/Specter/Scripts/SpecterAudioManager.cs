using UnityEngine;
using System.Collections.Generic;

public class SpecterAudioManager : MonoBehaviour {
  public static SpecterAudioManager Instance;
  
  private List<string> idleMovement;
  private List<string> idleVoices;
  private List<string> cauldronBulletSplash;
  private string lastIdleMovement;  
  private string lastIdleVoices;  

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Debug.LogWarning("Found more than one SPecterAudioManager in scene");
    }
  
    idleMovement = new List<string>();
    idleVoices = new List<string>();
    cauldronBulletSplash = new List<string>();

    idleMovement.Add("Specter__Idle__1");
    idleMovement.Add("Specter__Idle__2");
    idleMovement.Add("Specter__Idle__3");
    idleMovement.Add("Specter__Idle__4");
    idleMovement.Add("Specter__Idle__5");

    idleVoices.Add("Specter__IdleVoice__1");
    idleVoices.Add("Specter__IdleVoice__2");
    idleVoices.Add("Specter__IdleVoice__3");
    
    cauldronBulletSplash.Add("Specter__Cauldron__Bullet__1");
    cauldronBulletSplash.Add("Specter__Cauldron__Bullet__2");
    cauldronBulletSplash.Add("Specter__Cauldron__Bullet__3");
    cauldronBulletSplash.Add("Specter__Cauldron__Bullet__4");
  }
  public void IntroWhoosh() {
    // Start: 6
    AudioManager.Instance.Play("Specter__Intro1");
  }

  public void IntroSwing() {
    // Start: 15
    AudioManager.Instance.Play("Specter__Intro2");
  }

  public void PlayIdleSound() {
    StopIdleSound();
    
    lastIdleMovement = idleMovement[Random.Range(0, idleMovement.Count)];
    lastIdleVoices = idleVoices[Random.Range(0, idleVoices.Count)];
    
    AudioManager.Instance.Play(lastIdleMovement);
    AudioManager.Instance.Play(lastIdleVoices);
  }

  public void StopIdleSound() {
    if (lastIdleMovement != null && lastIdleVoices != null) {
      AudioManager.Instance.Stop(lastIdleMovement);
      AudioManager.Instance.Stop(lastIdleVoices);
    }
  }

  public void CannonTransformSound() {
    AudioManager.Instance.Play("Specter__Cannon__Transform");
  }

  public void CannonShootSound() {
    AudioManager.Instance.Play("Specter__Cannon__Shoot");
  }
  
  public void PlaySickleSound() {
    // AudioManager.Instance.Play("Specter__Cannon__Sickle");
  }
  public void StopSickleSound() {
    // AudioManager.Instance.Stop("Specter__Cannon__Sickle");
  }

  public void PortalOpen() {
    // 1-18
    AudioManager.Instance.Play("Specter__Portal__1");
  }
  public void PortalWhoosh() {
    // 8-
    AudioManager.Instance.Play("Specter__Portal__2");
  }
  
  public void PortalWhoosh2() {
    // 26
    AudioManager.Instance.Play("Specter__Portal__3");
  }

  public void PortaStratch() {
    // 34
    AudioManager.Instance.Play("Specter__Portal__4");
  }

  public void CauldronPortal() {
    // 16
    AudioManager.Instance.Play("Specter__Cauldron__Portal");
  }
  public void CauldronBonk() {
    AudioManager.Instance.Play("Specter__Cauldron__Bonk");
  }
  public void CauldronSlime() {
    AudioManager.Instance.Play("Specter__Cauldron__Slime");
  }
  public void CauldronMagic() {
    AudioManager.Instance.Play("Specter__Cauldron__Magic");
  }

  public void CauldronBulletSplash() {
    AudioManager.Instance.Play(cauldronBulletSplash[Random.Range(0, cauldronBulletSplash.Count)]);
  }

  public void TreeGrunt() {
    AudioManager.Instance.Play("Tree__Grunt");
  }
  
  public void TreeMovement() {
    AudioManager.Instance.Play("Tree__Movement");
  }

  public void SpecterClockDing() {
    AudioManager.Instance.Play("Specter__Clock__Ding");
  }

  public void SpecterClockDong() {
    AudioManager.Instance.Play("Specter__Clock__Dong");
  }
}
