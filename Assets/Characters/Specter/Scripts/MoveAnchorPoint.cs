using UnityEngine;

public class MoveAnchorPoint : MonoBehaviour {
  private float _offset;
  private Vector3 _originalPosition;
  private float _counter;
  private float _timer = 0.5f;
  
  private void Awake() {
    _originalPosition = transform.position;
  }
  
  private void FixedUpdate() {
    _counter += Time.deltaTime;
    if (_counter >= _timer) {
      AddOffset();
      _counter = 0f;
    }
  }  

  private void AddOffset() {
    Vector3 pos = _originalPosition;
    _offset = Random.Range(0.25f, 1f);
    pos.x += _offset;    
    pos.y += _offset;    
    transform.position = pos;
  }
}
