using UnityEngine;
using System.Collections.Generic;

/// This class manages the pool of sparks for the parry. And it also manages the creation and returning to pool
public class ParrySparksManager : MonoBehaviour {
  public string parrySparkName = "ParrySpark";
  private int poolParrySparkSize = 5;
  private GameObject parrySparkPrefab = null;
  private Queue<GameObject> parrySparkQueue = new Queue<GameObject>();

  private void Start() {
    parrySparkPrefab = Resources.Load<GameObject>(parrySparkName);
    InitializePool();
  }

  private void InitializePool() {
    for (int i = 0; i < poolParrySparkSize; i++) {
      GameObject spark = InstantiateSpark();
      spark.SetActive(false);
      parrySparkQueue.Enqueue(spark);
    }
  }

  private GameObject GetSpark() {
    if (parrySparkQueue.Count > 0) {
      GameObject spark = parrySparkQueue.Dequeue();
      spark.SetActive(true);
      return spark;
    } else {
      GameObject spark = InstantiateSpark();
      spark.SetActive(true);
      return spark;
    }
  }

  public void ReturnSpark(GameObject spark) {
    spark.SetActive(false);
    parrySparkQueue.Enqueue(spark);
  }

  private GameObject InstantiateSpark() {
    return Instantiate(parrySparkPrefab, BulletPoolManager.Instance.transform);
  }

  public void ShowSpark(Transform spawn) {
    GameObject spark = GetSpark();
    spark.transform.position = spawn.position;
  }
}
