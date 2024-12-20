using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCycler : MonoBehaviour {
  [Header("Overlay Settings")]
  public string resourcesFolder = "Overlays";
  public float cycleInterval = 0.1f;

  private List<Sprite> overlaySprites = new List<Sprite>();
  private Image overlayImage;
  private int currentIndex = 0;

  private void Start() {
    overlayImage = GetComponent<Image>();
    if (overlayImage == null) {
      Debug.LogError("OverlayCycler requires an Image component.");
      return;
    }

    Sprite[] sprites = Resources.LoadAll<Sprite>(resourcesFolder);
    if (sprites.Length == 0) {
      Debug.LogError("No sprites found in provided folder.");
      return;
    }

    overlaySprites.AddRange(sprites);
    StartCoroutine(CycleOverlays());
  }

  private IEnumerator CycleOverlays() {
    while (true) {
      overlayImage.sprite = overlaySprites[currentIndex];
      currentIndex = (currentIndex + 1) % overlaySprites.Count;
      yield return new WaitForSeconds(cycleInterval);
    }
  }
}
