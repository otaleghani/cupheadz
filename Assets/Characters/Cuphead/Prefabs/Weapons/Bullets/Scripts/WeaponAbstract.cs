using UnityEngine;

/// Abstract for the weapons. Handles the spawning mechanics for the different bullets
public abstract class WeaponManager : MonoBehaviour {
  public virtual float fireRate {get; protected set;} = 0.5f;
  public virtual string weaponPrefabName {get; protected set;} = "Peashooter__Bullet";
  public virtual string sparklePrefabName {get; protected set;} = "Peashooter__Sparkle";
  private GameObject weapon;
  private GameObject sparkle;
  private Bullet bullet;

  protected virtual void Start() {
    weapon = Resources.Load<GameObject>(weaponPrefabName);
    sparkle = Resources.Load<GameObject>(sparklePrefabName);
    bullet = weapon.GetComponent<Bullet>();
  }

  // I could even pass the position, and the game is set
  public virtual void Shoot(int x, int y, Transform spawn) {
    Debug.Log(x + " " + y);
    if (x == 0 && y == 0) {
      Debug.LogWarning("Shoot function called with zero direction.");
      return;
    }

    // Calculates directions and angle
    Vector2 direction = new Vector2(x, y).normalized;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    GameObject bulletInstance = Instantiate(weapon, spawn.position, Quaternion.Euler(0, 0, angle));
    GameObject sparkleInstance = Instantiate(sparkle, spawn.position, Quaternion.identity);
    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();

    if (rb != null) {
      Debug.Log(direction);
      rb.linearVelocity = direction * bullet.speed;
    } else {
      Debug.LogError("Bullet prefab does not have a Rigidbody2D component.");
    }
  }
}
