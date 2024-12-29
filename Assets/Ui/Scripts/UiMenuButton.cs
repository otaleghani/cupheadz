using UnityEngine;  
using UnityEngine.EventSystems;  
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  private TMP_Text text;
  private Color selecterColor = new Color(159 / 255f, 35 / 255f, 37 / 255f, 1f);
  private Color nonSelectedColor = new Color(74 / 255f, 58 / 255f, 59 / 255f, 1f);

  public void Awake() {
    text = GetComponentInChildren<TMP_Text>();
    text.color = nonSelectedColor;
  }

  public void OnPointerEnter(PointerEventData eventData) {
    text.color = selecterColor;
  }

  public void OnPointerExit(PointerEventData eventData) {
    text.color = nonSelectedColor;
  }
}
