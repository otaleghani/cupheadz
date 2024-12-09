using System.Collections.Generic;
//using UnityEngine;

[System.Serializable]
public class GameData {

  public enum Weapon {
    Peashooter,
    Spread,
    Chase,
    Lobber,
    Charge,
    Roundabout,
    Crackshot,
    Converge,
    Twist_up,
    None,
  }

  public int deathCount;
  public Dictionary<Weapon, bool> unlockedWeapon = new Dictionary<Weapon, bool>();
  public Dictionary<string, Weapon> equippedWeapon = new Dictionary<string, Weapon>();
  //Dictionary<Weapon, bool> bosses = new Dictionary<Weapon, bool>();


  public GameData() {
    this.deathCount = 0;

    this.unlockedWeapon.Add(Weapon.Peashooter, true);
    this.unlockedWeapon.Add(Weapon.Spread, false);
    this.unlockedWeapon.Add(Weapon.Chase, false);
    this.unlockedWeapon.Add(Weapon.Lobber, false);
    this.unlockedWeapon.Add(Weapon.Charge, false);
    this.unlockedWeapon.Add(Weapon.Roundabout, false);
    this.unlockedWeapon.Add(Weapon.Crackshot, false);
    this.unlockedWeapon.Add(Weapon.Converge, false);
    this.unlockedWeapon.Add(Weapon.Twist_up, false);

    this.equippedWeapon.Add("first", Weapon.Peashooter);
    this.equippedWeapon.Add("second", Weapon.None);

  }
}
