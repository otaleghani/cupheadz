using UnityEngine;

public class PeashooterWeaponManager : WeaponManager {
  public override float fireRate {get; protected set;} = 0.15f;
  public override string weaponPrefabName {get; protected set;} = "Peashooter__Bullet";
  public override string sparklePrefabName {get; protected set;} = "Peashooter__Sparkle";
  public override string exWeaponPrefabName {get; protected set;} = "Peashooter__ExBullet";
  
  protected override int PoolBulletSize {
    get { return base.PoolBulletSize; }
    set { base.PoolBulletSize = 20; }
  }
  protected override int PoolExBulletSize {
    get { return base.PoolBulletSize; }
    set { base.PoolBulletSize = 5; }
  }

  private float offset = 0.2f;
  private Vector3 spawnPosition;

  protected override void SpawnBulletFromPool(Vector2 direction, float angle, Transform spawn) {
    spawnPosition = spawn.position;
    if (direction.y == 0f && PlayerStateManager.instance.movementState is not PlayerCrouchState) {
      // If we are shooting on a straight line we want to alternate with bullets a little on the top
      // or the bottom
      spawnPosition = new Vector3(spawn.position.x, spawn.position.y + offset, spawn.position.z);
      offset *= -1f;
    }
    GameObject bulletInstance = GetBullet();
    bulletInstance.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0, 0, angle));
    Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
    rb.linearVelocity = new Vector2(direction.x * bulletData.speed, direction.y * bulletData.speed);
  }
}
