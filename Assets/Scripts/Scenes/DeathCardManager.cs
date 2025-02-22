using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathCardManager : MonoBehaviour {
  public static DeathCardManager Instance;
  public List<DeathCard> DeathCards;
  public DeathFlags DeathFlags;
  private GameObject _flagPrefab;
  private DeathCard _currentDeathCard;

  private float _containerWidth;
  
  private void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Debug.LogWarning("Found more than one DeathCardManager in scene.");
    }
    // DeathCards = new List<DeathCard>();
    _flagPrefab = Resources.Load<GameObject>("DeathFlags/DeathFlagPrefab");
    _containerWidth = transform.Find("DeathFlagContainer").gameObject.GetComponent<RectTransform>().sizeDelta.x;
  }

  // private void Start() {
  //   switch (FightSceneStateManager.Instance.CurrentScene) {
  //     case FightSceneStateManager.SceneName.Specter:
  //     DeathCards = Resources.LoadAll<DeathCard>("DeathCards/Specter").ToList();
  //     DeathFlags = Resources.Load<DeathFlags>("DeathFlags/Specter/Specter__DeathFlags");
  //     SpawnFlags();
  //     ToDisplay("Specter");
  //     break;
  //   }
  // }

  public void ToDisplay() {
    SpawnFlags();
    AudioManager.Instance.SlowDownSoundtrack();
    EventSystem.current.SetSelectedGameObject(transform.Find("Buttons/Retry").gameObject);
    _currentDeathCard = DeathCards[BossStateManager.Instance.BossPhase];
    transform.Find("Quote").gameObject.GetComponent<TMP_Text>().text = $"\"{_currentDeathCard.Phrase}\"";
    transform.Find("WantedImage").gameObject.GetComponent<Image>().sprite = _currentDeathCard.Sprite;
    StartCoroutine(MoveIndicator());
  }

  /// <summary>
  /// Spawns the different flags inside of the GameObject for the given scene
  /// </summary>
  private void SpawnFlags() {
    foreach (int point in DeathFlags.Points) {
      GameObject flag = Instantiate(_flagPrefab, transform.Find("DeathFlagContainer").transform);
      flag.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
      flag.GetComponent<RectTransform>().sizeDelta = new Vector2((point / 100f) * _containerWidth, 50);
      flag.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
      flag.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
      flag.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
    }
  }

  private float CalculateHealthPercentage(float max, float current) {
    float percentage = (current / max) * 100f;
    // return Mathf.Clamp(percentage, 0f, 100f);
    return Mathf.Abs(percentage - 100f);
  }

  private float CalculatePositionIndicator(float max, float current) {
    float percentage = CalculateHealthPercentage(max, current);
    float position = (percentage / 100f) * _containerWidth;
    return position;
  }

  private IEnumerator MoveIndicator() {
    float targetX = CalculatePositionIndicator(BossStateManager.Instance.Health, BossStateManager.Instance.CurrentHealth);
    
    RectTransform indicator = transform.Find("DeathFlagContainer/PlayerIndicator").GetComponent<RectTransform>();
    float duration = 2f;
    float elapsed = 0f;
    float startX = indicator.anchoredPosition.x;
    
    while (elapsed < duration) {
      float t = elapsed / duration;  
      float easedT = Mathf.Sin(t * Mathf.PI * 0.5f);
      
      float newX = Mathf.Lerp(startX, targetX, easedT);
      Vector2 pos = indicator.anchoredPosition;
      pos.x = newX;
      indicator.anchoredPosition = pos;

      elapsed += Time.deltaTime;
      yield return null;
    }
    Vector2 finalPos = indicator.anchoredPosition;
    finalPos.x = targetX;
    indicator.anchoredPosition = finalPos;   
  }

  public void ButtonRetry() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void ButtonExitToMap() {
    SceneManager.LoadScene("Map");
  }

  public void ButtonQuitGame() {
    SceneManager.LoadScene("SplashScreen");
  }
}
