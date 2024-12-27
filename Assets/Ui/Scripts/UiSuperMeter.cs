using UnityEngine;
//using UnityEngine.UI;

public class UiSuperMeter : MonoBehaviour {
  public GameObject one;
  public GameObject two;
  public GameObject three;
  public GameObject four;
  public GameObject five;

  private RectTransform oneImage;
  private RectTransform twoImage;
  private RectTransform threeImage;
  private RectTransform fourImage;
  private RectTransform fiveImage;

  private PlayerStateManager stateManager;

  private void Awake() {
    oneImage = one.GetComponent<RectTransform>();
    twoImage = two.GetComponent<RectTransform>();
    threeImage = three.GetComponent<RectTransform>();
    fourImage = four.GetComponent<RectTransform>();
    fiveImage = five.GetComponent<RectTransform>();

    stateManager = GameObject.Find("Cuphead").GetComponent<PlayerStateManager>();
  }

  private void OnEnable() {
    stateManager.OnPlayerSuperMeterChange += HandleSuperMeterChange;
  }
  private void OnDisable() {
    stateManager.OnPlayerSuperMeterChange += HandleSuperMeterChange;
  }

  // Fully visible is when localPositionY is 0
  // Fully invisible is when localPositionY is -40
  private void HandleSuperMeterChange(float meter) {
    if (meter >= 5f) {
      oneImage.localPosition = new Vector3(oneImage.localPosition.x, 0f, oneImage.localPosition.z);
      twoImage.localPosition = new Vector3(twoImage.localPosition.x, 0f, twoImage.localPosition.z);
      threeImage.localPosition = new Vector3(threeImage.localPosition.x, 0f, threeImage.localPosition.z);
      fourImage.localPosition = new Vector3(fourImage.localPosition.x, 0f, fourImage.localPosition.z);
      fiveImage.localPosition = new Vector3(fiveImage.localPosition.x, 0f, fiveImage.localPosition.z);
      return;
    }
    if (meter >= 4f) {
      oneImage.localPosition = new Vector3(oneImage.localPosition.x, 0f, oneImage.localPosition.z);
      twoImage.localPosition = new Vector3(twoImage.localPosition.x, 0f, twoImage.localPosition.z);
      threeImage.localPosition = new Vector3(threeImage.localPosition.x, 0f, threeImage.localPosition.z);
      fourImage.localPosition = new Vector3(fourImage.localPosition.x, 0f, fourImage.localPosition.z);
      fiveImage.localPosition = new Vector3(fiveImage.localPosition.x, -40f + (meter - 4f) * 40f, fiveImage.localPosition.z);
      return;
    }
    if (meter >= 3f) {
      oneImage.localPosition = new Vector3(oneImage.localPosition.x, 0f, oneImage.localPosition.z);
      twoImage.localPosition = new Vector3(twoImage.localPosition.x, 0f, twoImage.localPosition.z);
      threeImage.localPosition = new Vector3(threeImage.localPosition.x, 0f, threeImage.localPosition.z);
      fourImage.localPosition = new Vector3(fourImage.localPosition.x, -40f + (meter - 3f) * 40f, fourImage.localPosition.z);
      fiveImage.localPosition = new Vector3(fiveImage.localPosition.x, -40f, fiveImage.localPosition.z);
      return;
    }
    if (meter >= 2f) {
      oneImage.localPosition = new Vector3(oneImage.localPosition.x, 0f, oneImage.localPosition.z);
      twoImage.localPosition = new Vector3(twoImage.localPosition.x, 0f, twoImage.localPosition.z);
      threeImage.localPosition = new Vector3(threeImage.localPosition.x, -40f + (meter - 2f) * 40f, threeImage.localPosition.z);
      fourImage.localPosition = new Vector3(fourImage.localPosition.x, -40f, fourImage.localPosition.z);
      fiveImage.localPosition = new Vector3(fiveImage.localPosition.x, -40f, fiveImage.localPosition.z);
      return;
    }
    if (meter >= 1f) {
      oneImage.localPosition = new Vector3(oneImage.localPosition.x, 0f, oneImage.localPosition.z);
      twoImage.localPosition = new Vector3(twoImage.localPosition.x, -40f + (meter - 1f) * 40f, twoImage.localPosition.z);
      threeImage.localPosition = new Vector3(threeImage.localPosition.x, -40f, threeImage.localPosition.z);
      fourImage.localPosition = new Vector3(fourImage.localPosition.x, -40f, fourImage.localPosition.z);
      fiveImage.localPosition = new Vector3(fiveImage.localPosition.x, -40f, fiveImage.localPosition.z);
      return;
    }
    oneImage.localPosition = new Vector3(oneImage.localPosition.x, -40f + (meter) * 40f, oneImage.localPosition.z);
    twoImage.localPosition = new Vector3(twoImage.localPosition.x, -40f, twoImage.localPosition.z);
    threeImage.localPosition = new Vector3(threeImage.localPosition.x, -40f, threeImage.localPosition.z);
    fourImage.localPosition = new Vector3(fourImage.localPosition.x, -40f, fourImage.localPosition.z);
    fiveImage.localPosition = new Vector3(fiveImage.localPosition.x, -40f, fiveImage.localPosition.z);
  }
}
