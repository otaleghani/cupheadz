public class ChaserWeaponManager : WeaponManager {
  public override float fireRate {get; protected set;} = 0.1f;
  public override string weaponPrefabName {get; protected set;} = "Chaser__Bullet";
  public override string sparklePrefabName {get; protected set;} = "Chaser__Sparkle";
}
