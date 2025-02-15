using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {
  public static AudioManager Instance;
  public Sound[] Sounds;

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
      // s.Source.pitch = s.Pitch;
      s.Source.loop = s.Loop;
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
}
