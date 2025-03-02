using UnityEngine;
public class ChaserWeaponManager : WeaponManager {
  public override float fireRate {get; protected set;} = 0.18f;
  public override string weaponPrefabName {get; protected set;} = "Chaser__Bullet";
  public override string sparklePrefabName {get; protected set;} = "Chaser__Sparkle";
  public override string exWeaponPrefabName {get; protected set;} = "Chaser__ExBullet";
  
  // protected override void SpawnExBulletFromPool(Vector2 direction, float angle, Transform spawn) {
  //   GameObject exBulletInstance = GetExBullet();
  //   exBulletInstance.transform.position = new Vector3(spawn.position.x, spawn.position.y);
  //   exBulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
  // }

  private void SpawnExBulletFromPool(Vector2 direction, float angle, Transform spawn, float startAngle) {
    GameObject exBulletInstance = GetExBullet();
    exBulletInstance.transform.position = new Vector3(spawn.position.x, spawn.position.y);
    exBulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    
    exBulletInstance.GetComponent<ChaserExBullet>().currentAngle = startAngle;
  }

  public override void ExShoot(int x, int y, Transform spawn, Transform sfxSpawn) {
    if (x == 0 && y == 0) {
      Debug.LogWarning("Shoot function called with zero direction.");
      return;
    }

    Vector2 direction = GetDirection(x, y);
    float angle = GetAngle(direction.x, direction.y);
    SpawnExBulletFromPool(direction, angle, spawn, 0f);
    SpawnExBulletFromPool(direction, angle, spawn, 90f);
    SpawnExBulletFromPool(direction, angle, spawn, 180f);
    SpawnExBulletFromPool(direction, angle, spawn, 270f);

    SpawnExSfx(x, y, sfxSpawn);
  }
}
