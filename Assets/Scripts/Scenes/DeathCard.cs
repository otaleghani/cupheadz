using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New death card", menuName = "DeathCard/Card")]
public class DeathCard : ScriptableObject {
  [SerializeField] private string _name;
  [SerializeField] private Sprite _sprite;
  [SerializeField] private string _phrase;
  
  public string Name => _name;
  public Sprite Sprite => _sprite;
  public string Phrase => _phrase;
}

[CreateAssetMenu(fileName = "New death flags", menuName = "DeathCard/Flags")]
public class DeathFlags : ScriptableObject {
  [SerializeField] private List<int> _points; 
  
  public List<int> Points => _points;
}
