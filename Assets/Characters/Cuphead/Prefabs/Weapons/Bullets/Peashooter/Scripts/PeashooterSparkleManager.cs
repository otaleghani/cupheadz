using UnityEngine;

public class PeashooterSparkle : BulletSparkle {
  protected override void Awake() {
    weaponManager = GameObject.Find("Peashooter__Weapon").GetComponent<WeaponManager>();
  }
}
