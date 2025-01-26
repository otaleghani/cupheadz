using UnityEngine;
using TMPro;

public class BossHealthCounter : MonoBehaviour {
  private TMP_Text _displayText;

  private void Start() {
    _displayText = GetComponentInChildren<TMP_Text>();
    BossStateManager.Instance.OnDamage += HandleOnDamage;
    _displayText.text = BossStateManager.Instance.CurrentHealth.ToString() + " / " + BossStateManager.Instance.Health.ToString();
  }
  
  private void HandleOnDamage() {
    _displayText.text = BossStateManager.Instance.CurrentHealth.ToString() + " / " + BossStateManager.Instance.Health.ToString();
  }
}
