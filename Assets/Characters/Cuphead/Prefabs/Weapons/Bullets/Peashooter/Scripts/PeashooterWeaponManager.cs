public class PeashooterWeaponManager : WeaponManager {
  public override float fireRate {get; protected set;} = 0.1f;
  public override string weaponPrefabName {get; protected set;} = "Peashooter__Bullet";
  public override string sparklePrefabName {get; protected set;} = "Peashooter__Sparkle";
  
}
