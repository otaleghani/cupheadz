using UnityEngine;
public class PeashooterBullet : Bullet {

  protected override void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    weaponManager = GameObject.Find("Peashooter__Weapon").GetComponent<WeaponManager>();
  }


}
