using UnityEngine;

public class CauldronBulletManager : MonoBehaviour {
  private Animator _animator;
  private float _speed = -0.15f;
  private bool _isDropping = true;
  private float _elapsed = 1f;
  
  private void Awake() {
    _animator = GetComponent<Animator>();
    GetComponentInChildren<CauldronBulletGroundCheck>().GroundDetected += HandleGroundDetected;
    //GetComponentInChildren<CauldronBulletPlayerCheck>().PlayerDetected += HandlePlayerDetected;
  }

  private void Start() {}

  private void FixedUpdate() {
    if (_isDropping) {
      Drop();
    }
  }

  private void Drop() {
    //_elapsed += Time.deltaTime;
    Vector3 pos = new Vector3(transform.position.x, transform.position.y + _speed * _elapsed, transform.position.z);
    transform.position = pos;
  }

  private void HandleGroundDetected() {
    _animator.Play("Specter__CauldronBulletSplash");
    SpecterAudioManager.Instance.CauldronBulletSplash();
  }
  
  private void HandlePlayerDetected() {
    //Debug.Log("Helo");
  }
  
  public void OnSplashAnimationEnd() {
    Destroy(gameObject);
  }

  public void DropEnd() {
    _isDropping = false;
  }
}
