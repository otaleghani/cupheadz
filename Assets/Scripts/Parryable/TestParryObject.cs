using UnityEngine;

public class TestParryObject : MonoBehaviour, IParryable {
  private ParrySparksManager parrySparksManager;
  //private int counter = 0;
  private Sprite sprite;

  private void Awake() {
    parrySparksManager = GameObject.Find("ParrySparksManager").GetComponent<ParrySparksManager>();
  }

  public void OnParry() {
    parrySparksManager.ShowSpark(gameObject.transform);
    //StartCoroutine(ReActivateAfter(2));
    //gameObject.SetActive(false);
    ObjectResetter.instance.ResetAfter(gameObject, 2);
  }

  //private IEnumerator ReActivateAfter(int seconds) {
  //  sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
  //  gameObject.GetComponent<SpriteRenderer>().sprite = null;
  //  yield return new WaitForSeconds(seconds);
  //  gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
  //  Debug.Log("Got out");
  //}
}
