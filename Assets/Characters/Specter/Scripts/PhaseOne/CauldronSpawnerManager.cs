using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CauldronSpawnerManager : MonoBehaviour {
  public static CauldronSpawnerManager Instance;
  public List<GameObject> SpawnPoints;
  private float _cooldown = 0.3f;
  private GameObject _bulletPrefab;
  private CoroutineQueueManager _coroutineManager;
  private int _maxSpawnTimes = 3;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Debug.LogWarning("More than one CauldronSpawnerManager found in scene.");
    }
    _bulletPrefab = Resources.Load<GameObject>("SpecterCauldronBullet");
    if (_coroutineManager == null) {
      _coroutineManager = gameObject.AddComponent<CoroutineQueueManager>();
    }
  }

  public void SpawnBullets() {
    for (int count = 0; count < _maxSpawnTimes; count++) {
      ShuffleList();
      for (int i = 0; i < SpawnPoints.Count; i++) {
        _coroutineManager.EnqueueCoroutine(SpawnBullet(SpawnPoints[i]));
      }
    }
  }

  private void ShuffleList() {
    int n = SpawnPoints.Count;
    for (int i = n - 1; i > 0; i--) {
      int j = Random.Range(0, i + 1);
      GameObject temp = SpawnPoints[i];
      SpawnPoints[i] = SpawnPoints[j];
      SpawnPoints[j] = temp;
    }
  }

  private IEnumerator SpawnBullet(GameObject spawnPoint) {
    yield return new WaitForSeconds(_cooldown);
    GameObject bullet = Instantiate(_bulletPrefab);
    bullet.transform.position = spawnPoint.transform.position;
  }
}
