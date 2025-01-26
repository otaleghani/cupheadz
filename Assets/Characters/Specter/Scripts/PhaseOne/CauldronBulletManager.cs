using UnityEngine;

public class CauldronBulletManager : MonoBehaviour {
  private Animator _animator;
  
  private void Awake() {
    _animator = GetComponent<Animator>();
    GetComponentInChildren<CauldronBulletGroundCheck>().GroundDetected += HandleGroundDetected;
  }

  private void Start() { }

  private void HandleGroundDetected() {
    _animator.Play("Specter__CauldronBulletSplash");
  }
  
  public void OnSplashAnimationEnd() {
    Destroy(gameObject);
  }
}
