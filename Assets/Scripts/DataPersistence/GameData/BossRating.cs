public class BossRating {
  public enum Rating {
    S,
    Ap,
    A,
    Am,
    Bp,
    B,
    Bm,
    C,
    NaN,
  }

  public Rating rate;

  public BossRating() {
    this.rate = Rating.NaN;
  }

  public BossRating(Rating rate) {
    this.rate = rate;
  }
}
