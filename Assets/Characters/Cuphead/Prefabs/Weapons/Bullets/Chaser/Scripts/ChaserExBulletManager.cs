using UnityEngine;

public class ChaserExBullet : ExBullet {
  protected override void Awake() {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    weaponManager = GameObject.Find("Chaser__Weapon").GetComponent<WeaponManager>();
  }
}
