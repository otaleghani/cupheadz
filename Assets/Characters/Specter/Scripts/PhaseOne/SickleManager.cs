using UnityEngine;
using System.Collections;

public class SickleManager : MonoBehaviour {
  private float _exitScreenDuration = 1f;
  private float _levelDuration = 0.3f;
  private float _enterScreenDuration = 4f;
  private float _sineHeight = 1.5f;
  private int _waveCount = 3;
  private CoroutineQueueManager _coroutineManager;
  
  private void Awake() {
    _coroutineManager = gameObject.GetComponent<CoroutineQueueManager>();
    if (_coroutineManager == null) {
      _coroutineManager = gameObject.AddComponent<CoroutineQueueManager>();
    }
  }

  public void StartSickle(bool isLeft, int slowFactor, int attackType) {
    _coroutineManager.EnqueueCoroutine(Move(GameObject.Find("Specter/SickleExitPoint").transform.position, _exitScreenDuration, 0));
    if (isLeft) {
      _coroutineManager.EnqueueCoroutine(Move(
        GameObject.Find("SpecterMovePoints/Sickle/SickleScreenRight").transform.position,
        _levelDuration * slowFactor,
        slowFactor * 3
      ));
      // Here check attack type, then do the right one
      _coroutineManager.EnqueueCoroutine(MoveSine(
        GameObject.Find("SpecterMovePoints/Sickle/SickleScreenLeft").transform.position,
        _enterScreenDuration,
        _sineHeight,
        _waveCount,
        -slowFactor
      ));
    } else {
      _coroutineManager.EnqueueCoroutine(Move(
        GameObject.Find("SpecterMovePoints/Sickle/SickleScreenLeft").transform.position,
        _levelDuration * slowFactor,
        -slowFactor * 3
      ));
      _coroutineManager.EnqueueCoroutine(MoveSine(
        GameObject.Find("SpecterMovePoints/Sickle/SickleScreenRight").transform.position,
        _enterScreenDuration,
        _sineHeight,
        _waveCount,
        slowFactor
      ));
    }
  }

  private IEnumerator Move(Vector3 dest, float duration, int offset) {
    Vector3 startPos = transform.position;
    Vector3 destination = new Vector3(dest.x + offset, dest.y, dest.z);
    float elapsed = 0f;
    while (elapsed < duration) {
      float t = Mathf.Clamp01(elapsed / duration);
      transform.position = Vector3.Lerp(startPos, destination, t);
      elapsed += Time.deltaTime;
      yield return null;
    }
    transform.position = destination;
  }
  
  private IEnumerator MoveSine(Vector3 destination, float duration, float height, int waveCount, int offset) {
    Vector3 startPos = transform.position;
    Vector3 horizontalStart = new Vector3(startPos.x, 0, startPos.z);
    Vector3 horizontalDestination = new Vector3(destination.x + offset, 0, destination.z);
    float elapsed = 0f;
    while (elapsed < duration) {
      float t = Mathf.Clamp01(elapsed / duration);    
      Vector3 horizontalPosition = Vector3.Lerp(horizontalStart, horizontalDestination, t);
      
      float sineArgument = t * waveCount * Mathf.PI * 2;
      float sineValue = Mathf.Sin(sineArgument);
      float verticalOffset = height * sineValue;
            
      transform.position = new Vector3(horizontalPosition.x, startPos.y + verticalOffset, horizontalPosition.z);
      elapsed += Time.deltaTime;
      yield return null;
    }
    transform.position = destination;
    Destroy(gameObject);
  }

  //private IEnumerator MoveBoomerang(Vector3 destination, float duration, float height, )
}
