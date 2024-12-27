using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiHealthCounter : MonoBehaviour {
  private PlayerStateManager stateManager;
  private TMP_Text healthText;
  private Image panelBackground;
  private bool isFlickering = false;
  private int counter = 0;
  private Color yellow = new Color(239 / 255f, 220 / 255f, 57 / 255f, 1f);
  private Color white = new Color(1f, 1f, 1f, 1f);
  private Color red = new Color(244 / 255f, 8 / 255f, 51 / 255f, 1f);
  private Color gray = new Color(211 / 255f, 209 / 255f, 211 / 255f, 1f);

  private void Awake() {
    stateManager = GameObject.Find("Cuphead").GetComponent<PlayerStateManager>();
    healthText = GetComponentInChildren<TMP_Text>();
    panelBackground = GetComponent<Image>();
    panelBackground.color = yellow;
  }
  private void OnEnable() {
    stateManager.OnPlayerHealthChange += HandlePlayerHealthChange;
  }
  private void OnDisable() {
    stateManager.OnPlayerHealthChange -= HandlePlayerHealthChange;
  }

  private void FixedUpdate() {
    if (isFlickering && counter == 0 && stateManager.hearts != 0) {
      Flicker();
      counter += 1;
    } else {
      counter += 1;
      if (counter == 3) counter = 0;
    }
  }

  private void HandlePlayerHealthChange(int newHealth) {
    if (newHealth == 1) isFlickering = true;
    if (newHealth <= 0) {
      healthText.text = "DEAD";
      panelBackground.color = gray;
    } else {
      healthText.text = "HP.   " + newHealth.ToString();
    }
  }

  private void Flicker() {
    panelBackground.color = panelBackground.color == white ? panelBackground.color = red : panelBackground.color = white ;
  }
}
