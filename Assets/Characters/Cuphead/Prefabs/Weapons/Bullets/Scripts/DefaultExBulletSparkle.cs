using UnityEngine;
public class DefaultExBulletSparkle : ExBulletSparkle {
  protected override void Awake() {
    rb = GetComponent<Rigidbody2D>();
    weaponManager = GameObject.Find("Peashooter__Weapon").GetComponent<WeaponManager>();
  }
}
