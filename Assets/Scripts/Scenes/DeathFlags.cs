using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New death flags", menuName = "DeathCard/Flags")]
public class DeathFlags : ScriptableObject {
  public List<int> Points; 
}
