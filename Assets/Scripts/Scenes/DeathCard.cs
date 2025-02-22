using UnityEngine;

[CreateAssetMenu(fileName = "New death card", menuName = "DeathCard/Card")]
public class DeathCard : ScriptableObject {
  [SerializeField] private string _name;
  [SerializeField] private Sprite _sprite;
  [SerializeField] private string _phrase;
  
  public string Name => _name;
  public Sprite Sprite => _sprite;
  public string Phrase => _phrase;
}
