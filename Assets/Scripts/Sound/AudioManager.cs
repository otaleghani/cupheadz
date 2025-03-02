using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour {
  public static AudioManager Instance;
  public Sound[] Sounds;
  private AudioSource _soundtrack;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Debug.LogWarning("Found more than one instance of AudioManager");
    }
    foreach (Sound s in Sounds) {
      s.Source = gameObject.AddComponent<AudioSource>();
      s.Source.clip = s.Clip;
      s.Source.volume = s.Volume;
      s.Source.pitch = s.Pitch;
      s.Source.loop = s.Loop;
      if (s.Name == "Soundtrack") _soundtrack = s.Source; 
    }
  }
  
  public void Play(string name) {
    Sound s = Array.Find(Sounds, sound => sound.Name == name);
    if (s == null) {
      Debug.LogWarning("Found a null sound, check your code for this sound: " + name);
      return;
    }
    s.Source.Play();
  }

  public void Stop(string name) {
    Sound s = Array.Find(Sounds, sound => sound.Name == name);
    if (s == null) {
      Debug.LogWarning("Found a null sound, check your code for this sound: " + name);
      return;
    }
    s.Source.Stop();
  }

  public void SlowDownSoundtrack() {
    // _soundtrack.pitch = 0.5f;
    StartCoroutine(PitchDown());
    foreach (Sound s in Sounds) {
      if (s.Name != "Soundtrack") {
        s.Source.volume = 0f;
      }
    }

    foreach (TreeManager tree in FindObjectsByType<TreeManager>(FindObjectsSortMode.None)) {
      tree.NoMoreSounds();
    }
  }

  private IEnumerator PitchDown() {
    if (_soundtrack.pitch == 1f) {
      float elapsed = 0f;
      float duration = 2f;
      float targetPitch = 0.7f;
      float startVolume = _soundtrack.volume;
      float targetVolume = 1f;
      float startPitch = 1f;
      
      float lowPassStart = 22000f;
      float lowPassTarget = 1500f;
      float highPassStart = 10f;
      float highPassTarget = 1500f;
      float distorStart = 0f;
      float distorTarget = 0.5f;
      
      AudioLowPassFilter lowPassFilter = _soundtrack.GetComponent<AudioLowPassFilter>();
      if (lowPassFilter == null) lowPassFilter = _soundtrack.gameObject.AddComponent<AudioLowPassFilter>();
      AudioHighPassFilter highPassFilter = _soundtrack.GetComponent<AudioHighPassFilter>();
      if (highPassFilter == null) highPassFilter = _soundtrack.gameObject.AddComponent<AudioHighPassFilter>();
      AudioDistortionFilter distortion = _soundtrack.GetComponent<AudioDistortionFilter>();
      if (distortion == null) distortion = _soundtrack.gameObject.AddComponent<AudioDistortionFilter>();
      
      lowPassFilter.lowpassResonanceQ = 1f;
      highPassFilter.highpassResonanceQ = 1f;

      while (elapsed <= duration) {
        float t = elapsed / duration;
        _soundtrack.pitch = Mathf.Lerp(startPitch, targetPitch, t);
        _soundtrack.volume = Mathf.Lerp(startVolume, targetVolume, t);
        lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassStart, lowPassTarget, t);
        highPassFilter.cutoffFrequency = Mathf.Lerp(highPassStart, highPassTarget, t);
        distortion.distortionLevel = Mathf.Lerp(distorStart, distorTarget, t);
        elapsed += Time.deltaTime;
        yield return null;
      }
      _soundtrack.pitch = targetPitch;

      lowPassFilter.cutoffFrequency = 1500f;
      highPassFilter.cutoffFrequency = 1500f;
      distortion.distortionLevel = 0.5f;
    }
  }
}
