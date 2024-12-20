public class PeashooterWeaponManager : WeaponManager {
  // This is just an example of a simple override
  public override float fireRate {get; protected set;} = 0.5f;
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
}
