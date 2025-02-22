using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OverlayCycler : MonoBehaviour {
  [Header("Overlay Settings")]
  
  public string resourcesFolder = "Overlays";
  public float cycleInterval = 0.1f;

  // private List<Sprite> overlaySprites = new List<Sprite>();
  private List<Texture> overlaySprites = new List<Texture>();
  private Image overlayImage;
  private int currentIndex = 0;

  // Film grain
  private FilmGrain filmGrain;

  private void Start() {
    // overlayImage = GetComponent<Image>();
    // if (overlayImage == null) {
    //   Debug.LogError("OverlayCycler requires an Image component.");
    //   return;
    // }

    // Sprite[] sprites = Resources.LoadAll<Sprite>(resourcesFolder);
    Texture[] sprites = Resources.LoadAll<Texture>(resourcesFolder);
    if (sprites.Length == 0) {
      Debug.LogError("No sprites found in provided folder.");
      return;
    }

    overlaySprites.AddRange(sprites);
    // StartCoroutine(CycleOverlays());

    // FILM GRAIN
    Volume globalVolume = GetComponent<Volume>();
    if (globalVolume != null && globalVolume.profile.TryGet<FilmGrain>(out filmGrain)) {
      filmGrain.texture.overrideState = true;
    } else {
      Debug.LogWarning("Film Grain effect not found in the Volume profile.");
    }
    StartCoroutine(CycleOverlaysFilmGrain());
  }

  // private IEnumerator CycleOverlays() {
  //   while (true) {
  //     overlayImage.sprite = overlaySprites[currentIndex];
  //     currentIndex = (currentIndex + 1) % overlaySprites.Count;
  //     yield return new WaitForSeconds(cycleInterval);
  //   }
  // }

  private IEnumerator CycleOverlaysFilmGrain() {
    while (true) {
      filmGrain.texture.value = overlaySprites[currentIndex];
      currentIndex = (currentIndex + 1) % overlaySprites.Count;
      yield return new WaitForSeconds(cycleInterval);
    }
  }
}
