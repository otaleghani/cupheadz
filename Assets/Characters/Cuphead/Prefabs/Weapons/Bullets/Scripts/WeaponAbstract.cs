using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the different pools of bullets, exBullets and sparkles.
/// </summary>
public abstract class WeaponManager : MonoBehaviour {
  public virtual float fireRate { get; protected set; } = 1f;
  public virtual float exFireRate { get; protected set; } = 2f;

  public virtual string weaponPrefabName {get; protected set;} = "Peashooter__Bullet";
  public virtual string sparklePrefabName {get; protected set;} = "Peashooter__Sparkle";
  public virtual string exWeaponPrefabName {get; protected set;} = "Peashooter__ExBullet";

  private GameObject bulletPrefab = null;
  private GameObject sparklePrefab = null;
  private GameObject exBulletPrefab = null;

  private Bullet bulletData;
  private ExBullet exBulletData;

  private Queue<GameObject> bulletPoolQueue = new Queue<GameObject>();
  private Queue<GameObject> sparklePoolQueue = new Queue<GameObject>();
  private Queue<GameObject> exBulletPoolQueue = new Queue<GameObject>();

  private int _poolBulletSize = 20;
  private int _poolExBulletSize = 6;

  protected virtual int PoolBulletSize {
    get { return _poolBulletSize; }
    set { _poolBulletSize = value; }
  }
  protected virtual int PoolExBulletSize {
    get { return _poolExBulletSize; }
    set { _poolExBulletSize = value; }
  }

  protected virtual void Start() {
    bulletPrefab = Resources.Load<GameObject>(weaponPrefabName);
    sparklePrefab = Resources.Load<GameObject>(sparklePrefabName);
    exBulletPrefab = Resources.Load<GameObject>(exWeaponPrefabName);
    bulletData = bulletPrefab.GetComponent<Bullet>();
    exBulletData = exBulletPrefab.GetComponent<ExBullet>();

    InitializePools();
  }

  /// <summary>
  /// Calculates the direction and the angle, then activates a bullet from the pool
  /// </summary>
  public virtual void ExShoot(int x, int y, Transform spawn) {
    if (x == 0 && y == 0) {
      Debug.LogWarning("Shoot function called with zero direction.");
      return;
    }

    Vector2 direction = GetDirection(x, y);
    float angle = GetAngle(direction.x, direction.y);
    SpawnExBUlletFromPool(direction, angle, spawn);
  }

  /// <summary>
  /// Calculates the direction and the angle, then activates a bullet from the pool
  /// </summary>
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

  /// <summary>
  /// Activates a Bullet from the pool, setting it's direction, rotation and velocity
  /// </summary>
  protected virtual void SpawnBulletFromPool(Vector2 direction, float angle, Transform spawn) {
    GameObject bulletInstance = GetBullet();
    bulletInstance.transform.SetPositionAndRotation(spawn.position, Quaternion.Euler(0, 0, angle));
    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
    rb.linearVelocity = new Vector2(direction.x * bulletData.speed, direction.y * bulletData.speed);
  }

  /// <summary>
  /// Activates an ExBullet from the pool, setting it's direction, rotation and velocity
  /// </summary>
  protected virtual void SpawnExBUlletFromPool(Vector2 direction, float angle, Transform spawn) {
    GameObject exBulletInstance = GetExBullet();
    exBulletInstance.transform.position = spawn.position;
    exBulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    Rigidbody2D rb = exBulletInstance.GetComponent<Rigidbody2D>();
    rb.linearVelocity = new Vector2(direction.x * bulletData.speed, direction.y * bulletData.speed);
  }

  /// <summary>
  /// Activates a Sparkle from the pool, setting it's direction, rotation and velocity
  /// </summary>
  protected virtual void SpawnSparkleFromPool(float angle, Transform spawn) {
    GameObject sparkleInstance = GetSparkle();
    sparkleInstance.transform.SetPositionAndRotation(spawn.position, Quaternion.Euler(0, 0, angle));
  }

  /// <summary>
  /// Instantiate a Game Object, deactivates it and then places it the respective pool
  /// </summary>
  protected void InitializePools() {
    for (int i = 0; i < _poolBulletSize; i++) {
      GameObject bullet = InstantiateBullet();
      GameObject sparkle = InstantiateSparkle();
      bullet.SetActive(false);
      sparkle.SetActive(false);
      bulletPoolQueue.Enqueue(bullet);
      sparklePoolQueue.Enqueue(sparkle);
    }
    for (int i = 0; i < _poolExBulletSize; i++) {
      GameObject exBullet = InstantiateExBullet();
      exBullet.SetActive(false);
      exBulletPoolQueue.Enqueue(exBullet);
    }
  }

  /// <summary>
  /// Methods used to instanciate the game objects
  /// </summary>
  protected GameObject InstantiateBullet() {
    return Instantiate(bulletPrefab, BulletPoolManager.Instance.transform);
  }
  protected GameObject InstantiateSparkle() {
    return Instantiate(sparklePrefab, BulletPoolManager.Instance.transform);
  }
  protected GameObject InstantiateExBullet() {
    return Instantiate(exBulletPrefab, BulletPoolManager.Instance.transform);
  }


  /// <summary>
  /// Methods used to get from the pool the respective GameObject. If it doesn't exist, 
  /// it creates a new one
  /// </summary>
  protected GameObject GetBullet() {
    if (bulletPoolQueue.Count > 0) {
      GameObject bullet = bulletPoolQueue.Dequeue();
      bullet.SetActive(true);
      return bullet;
    } else {
      GameObject bullet = InstantiateBullet();
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
