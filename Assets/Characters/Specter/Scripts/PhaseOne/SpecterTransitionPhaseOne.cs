using UnityEngine;
//using System.Collections;

public class SpecterTransitionPhaseOne : MonoBehaviour, IBossAction {
  public void Enter() {
    SpecterStateManager.Instance.ChangeAnimation("Phase1__Intro"); 
  }
  
  public void Update() {
    // Here I should add a method to move 
  }
  
  public void Exit() {}
}
