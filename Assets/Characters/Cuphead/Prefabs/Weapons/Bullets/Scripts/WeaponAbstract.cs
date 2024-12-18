using UnityEngine;
using System.Collections.Generic;

/// Abstract for the weapons. Handles the spawning mechanics for the different bullets
public abstract class WeaponManager : MonoBehaviour {
  public virtual float fireRate {get; protected set;} = 1f;
  public virtual float exFireRate {get; protected set;} = 2f;
  public virtual string weaponPrefabName {get; protected set;} = "Peashooter__Bullet";
  public virtual string sparklePrefabName {get; protected set;} = "Peashooter__Sparkle";
  public virtual string exWeaponPrefabName {get; protected set;} = "Peashooter__ExBullet";
  private GameObject bulletPrefab = null;
  private GameObject sparklePrefab = null;
  private GameObject exBulletPrefab = null;
  //private GameObject exSparklePrefab = null;
  private Bullet bulletData;
  private ExBullet exBulletData;

  // Pool
  private Queue<GameObject> bulletPoolQueue = new Queue<GameObject>();
  private Queue<GameObject> sparklePoolQueue = new Queue<GameObject>();
  private Queue<GameObject> exBulletPoolQueue = new Queue<GameObject>();
  private int poolBulletSize = 20;
  private int poolExBulletSize = 6;

  protected virtual void Start() {
    bulletPrefab = Resources.Load<GameObject>(weaponPrefabName);
    sparklePrefab = Resources.Load<GameObject>(sparklePrefabName);
    exBulletPrefab = Resources.Load<GameObject>(exWeaponPrefabName);
    bulletData = bulletPrefab.GetComponent<Bullet>();
    exBulletData = exBulletPrefab.GetComponent<ExBullet>();

    InitializePools();
  }

  public virtual void ExShoot(int x, int y, Transform spawn) {
    if (x == 0 && y == 0) {
      Debug.LogWarning("Shoot function called with zero direction.");
      return;
    }

    Vector2 direction = GetDirection(x, y);
    float angle = GetAngle(direction.x, direction.y);
    
    SpawnExBUlletFromPool(direction, angle, spawn);
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

    // Try like this by adding the transform position and rotation like this 
    bulletInstance.transform.position = spawn.position;
    bulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    bulletInstance.transform.SetPositionAndRotation(spawn.position, Quaternion.Euler(0, 0, angle));
    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
    rb.linearVelocity = new Vector2(direction.x * 20f, rb.linearVelocityY);

    //if (rb != null) {
    //  rb.linearVelocity = new Vector2(direction.x * bulletData.speed, direction.y);
    //} else {
    //  Debug.LogError("Bullet prefab does not have a Rigidbody2D component.");
    //}
    //bulletInstance.SetActive(true);
  }

  protected virtual void SpawnExBUlletFromPool(Vector2 direction, float angle, Transform spawn) {
    GameObject exBulletInstance = GetExBullet();
    exBulletInstance.transform.position = spawn.position;
    exBulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    Rigidbody2D rb = exBulletInstance.GetComponent<Rigidbody2D>();
    if (rb != null) {
      rb.linearVelocity = new Vector2(direction.x * bulletData.speed, direction.y);
    } else {
      Debug.LogError("Bullet prefab does not have a Rigidbody2D component.");
    }
    exBulletInstance.SetActive(true);
  }

  protected virtual void SpawnSparkleFromPool(float angle, Transform spawn) {
    GameObject sparkleInstance = GetSparkle();
    sparkleInstance.transform.SetPositionAndRotation(spawn.position, Quaternion.Euler(0, 0, angle));
  }

  protected void InitializePools() {
    for (int i = 0; i < poolBulletSize; i++) {
      GameObject bullet = InstantiateBullet();
      GameObject sparkle = InstantiateSparkle();
      bullet.SetActive(false);
      sparkle.SetActive(false);
      bulletPoolQueue.Enqueue(bullet);
      sparklePoolQueue.Enqueue(sparkle);
    }
    for (int i = 0; i < poolExBulletSize; i++) {
      GameObject exBullet = InstantiateExBullet();
      exBullet.SetActive(false);
      exBulletPoolQueue.Enqueue(exBullet);
    }
  }

  protected GameObject InstantiateBullet() {
    return Instantiate(bulletPrefab, BulletPoolManager.Instance.transform);
  }
  protected GameObject InstantiateSparkle() {
    return Instantiate(sparklePrefab, BulletPoolManager.Instance.transform);
  }
  protected GameObject InstantiateExBullet() {
    return Instantiate(exBulletPrefab, BulletPoolManager.Instance.transform);
  }

  protected GameObject GetBullet() {
    if (bulletPoolQueue.Count > 0) {
      GameObject bullet = bulletPoolQueue.Dequeue();
      bullet.SetActive(true);
      Debug.Log(bullet.activeSelf);
      return bullet;
    } else {
      GameObject bullet = InstantiateBullet();
      bullet.SetActive(true);
      //Debug.Log("New bullet created");
      return bullet;
    }
  }

  public void ReturnBullet(GameObject bullet) {
    bullet.SetActive(false);
    bulletPoolQueue.Enqueue(bullet);
    Debug.Log($"Bullet returned to pool. Pool size: {bulletPoolQueue.Count}");
  }

  protected GameObject GetSparkle() {
    if (sparklePoolQueue.Count > 0) {
      GameObject sparkle = sparklePoolQueue.Dequeue();
      sparkle.SetActive(true);
      return sparkle;
    } else {
      GameObject sparkle = InstantiateSparkle();
      sparkle.SetActive(true);
      return sparkle;
    }
  }

  public void ReturnSparkle(GameObject sparkle) {
    sparkle.SetActive(false);
    sparklePoolQueue.Enqueue(sparkle);
  }

  protected GameObject GetExBullet() {
    if (exBulletPoolQueue.Count > 0) {
      GameObject exBullet = exBulletPoolQueue.Dequeue();
      exBullet.SetActive(true);
      return exBullet;
    } else {
      GameObject exBullet = InstantiateExBullet();
      exBullet.SetActive(true);
      return exBullet;
    }
  }

  public void ReturnExBullet(GameObject exBullet) {
    exBullet.SetActive(false);
    exBulletPoolQueue.Enqueue(exBullet);
  }
}
