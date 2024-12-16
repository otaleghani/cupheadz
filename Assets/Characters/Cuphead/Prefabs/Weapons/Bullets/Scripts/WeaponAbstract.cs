using UnityEngine;
using System.Collections.Generic;

/// Abstract for the weapons. Handles the spawning mechanics for the different bullets
public abstract class WeaponManager : MonoBehaviour {
  //public WeaponManager weaonManager
  public virtual float fireRate {get; protected set;} = 1f;
  public virtual string weaponPrefabName {get; protected set;} = "Peashooter__Bullet";
  public virtual string sparklePrefabName {get; protected set;} = "Peashooter__Sparkle";
  private GameObject bulletPrefab = null;
  private GameObject sparklePrefab;
  private Bullet bulletData;

  // Pool
  private Queue<GameObject> bulletPoolQueue = new Queue<GameObject>();
  private Queue<GameObject> sparklePoolQueue = new Queue<GameObject>();
  private int poolSize = 20;

  protected virtual void Start() {
    bulletPrefab = Resources.Load<GameObject>(weaponPrefabName);
    sparklePrefab = Resources.Load<GameObject>(sparklePrefabName);
    bulletData = bulletPrefab.GetComponent<Bullet>();

    InitializePools();
  }

  public virtual void Shoot(int x, int y, Transform spawn) {
    if (x == 0 && y == 0) {
      Debug.LogWarning("Shoot function called with zero direction.");
      return;
    }

    Vector2 direction = GetDirection(x, y);
    float angle = GetAngle(direction.x, direction.y);

    SpawnBulletFromPool(direction, angle, spawn);
    SpawnSparkleFromPool(angle, spawn);
  }

  public Vector2 GetDirection(int x, int y) {
    return new Vector2(x, y).normalized;
  }
  public float GetAngle(float x, float y) {
    return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
  }

  protected virtual void SpawnBulletFromPool(Vector2 direction, float angle, Transform spawn) {
    GameObject bulletInstance = GetBullet();
    bulletInstance.transform.SetPositionAndRotation(spawn.position, Quaternion.Euler(0, 0, angle));
    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();

    if (rb != null) {
      rb.linearVelocity = direction * bulletData.speed;
    } else {
      Debug.LogError("Bullet prefab does not have a Rigidbody2D component.");
    }
  }

  protected virtual void SpawnSparkleFromPool(float angle, Transform spawn) {
    GameObject sparkleInstance = GetSparkle();
    sparkleInstance.transform.SetPositionAndRotation(spawn.position, Quaternion.Euler(0, 0, angle));

  }

  protected void InitializePools() {
    for (int i = 0; i < poolSize; i++) {
      GameObject bullet = Instantiate(bulletPrefab, transform);
      GameObject sparkle = Instantiate(sparklePrefab, transform);
      bullet.SetActive(false);
      sparkle.SetActive(false);
      bulletPoolQueue.Enqueue(bullet);
      sparklePoolQueue.Enqueue(sparkle);
    }
  }

  protected GameObject GetBullet() {
    if (bulletPoolQueue.Count > 0) {
      GameObject bullet = bulletPoolQueue.Dequeue();
      bullet.SetActive(true);
      return bullet;
    } else {
      GameObject bullet = Instantiate(bulletPrefab, transform);
      bullet.SetActive(true);
      return bullet;
    }
  }

  public void ReturnBullet(GameObject bullet) {
    bullet.SetActive(false);
    bulletPoolQueue.Enqueue(bullet);
  }

  protected GameObject GetSparkle() {
    if (sparklePoolQueue.Count > 0) {
      GameObject sparkle = sparklePoolQueue.Dequeue();
      sparkle.SetActive(true);
      return sparkle;
    } else {
      GameObject sparkle = Instantiate(sparklePrefab, transform);
      sparkle.SetActive(true);
      return sparkle;
    }
  }

  public void ReturnSparkle(GameObject sparkle) {
    sparkle.SetActive(false);
    sparklePoolQueue.Enqueue(sparkle);
  }
}
