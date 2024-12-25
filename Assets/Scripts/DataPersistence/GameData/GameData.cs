using System.Collections.Generic;

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

  public enum Charm {
    None,
    Heart,
    SmokeBomb,
    Sugar,
    Coffee,
    TwinHeart,
    Wheatstone,
    AstralCookie,
    HeartRing,
    BrokenRelic,
    CursedRelic,
    DivineRelic
  }

  public enum Boss {
    Specter
  }

  public enum Super {
    EnergyBeam,
    Invincibility,
    GiantGhost,
    None
  }

  public int deathCount;
  public Dictionary<Weapon, bool> unlockedWeapon = new Dictionary<Weapon, bool>();
  public Dictionary<string, Weapon> equippedWeapon = new Dictionary<string, Weapon>();
  public Dictionary<Charm, bool> unlockedCharm = new Dictionary<Charm, bool>();
  public Dictionary<Charm, bool> equippedCharm = new Dictionary<Charm, bool>();

  public Dictionary<Boss, bool> defeatedBoss = new Dictionary<Boss, bool>();
  public Dictionary<Boss, BossRating> defeatedBossRating = new Dictionary<Boss, BossRating>();

  public Dictionary<Super, bool> unlockedSupers = new Dictionary<Super, bool>();
  public Super equippedSuper;

  public GameData() {
    this.deathCount = 0;

    this.unlockedSupers.Add(Super.EnergyBeam, false);
    this.unlockedSupers.Add(Super.GiantGhost, false);
    this.unlockedSupers.Add(Super.Invincibility, false);

    // This one should be Super.None when creating a new game file.
    this.equippedSuper = Super.Invincibility;

    this.unlockedWeapon.Add(Weapon.None, true);
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
    this.equippedWeapon.Add("second", Weapon.Peashooter);

    this.unlockedCharm.Add(Charm.None, true);
    this.unlockedCharm.Add(Charm.Heart, false);
    this.unlockedCharm.Add(Charm.SmokeBomb, false);
    this.unlockedCharm.Add(Charm.Sugar, false);
    this.unlockedCharm.Add(Charm.Coffee, false);
    this.unlockedCharm.Add(Charm.TwinHeart, false);
    this.unlockedCharm.Add(Charm.Wheatstone, false);
    this.unlockedCharm.Add(Charm.AstralCookie, false);
    this.unlockedCharm.Add(Charm.HeartRing, false);
    this.unlockedCharm.Add(Charm.BrokenRelic, false);
    this.unlockedCharm.Add(Charm.CursedRelic, false);
    this.unlockedCharm.Add(Charm.DivineRelic, false);

    this.equippedCharm.Add(Charm.None, false);
    this.equippedCharm.Add(Charm.Heart, false);
    this.equippedCharm.Add(Charm.SmokeBomb, false);
    this.equippedCharm.Add(Charm.Sugar, false);
    this.equippedCharm.Add(Charm.Coffee, false);
    this.equippedCharm.Add(Charm.TwinHeart, false);
    this.equippedCharm.Add(Charm.Wheatstone, false);
    this.equippedCharm.Add(Charm.AstralCookie, false);
    this.equippedCharm.Add(Charm.HeartRing, false);
    this.equippedCharm.Add(Charm.BrokenRelic, false);
    this.equippedCharm.Add(Charm.CursedRelic, false);
    this.equippedCharm.Add(Charm.DivineRelic, false);

    // Todo: List all the bosses
    this.defeatedBoss.Add(Boss.Specter, false);
  }
}
