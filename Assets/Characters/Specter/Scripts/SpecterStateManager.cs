using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpecterStateManager : BossStateManager {
  public static event Action OnMoveEnd;
  private Coroutine _moveRoutine;
  private AnimationCurve easingArc = AnimationCurve.EaseInOut(0, 0, 1, 1);

  protected override void Awake() {
    base.Awake();
    _bossHealth = 1000;
    GenerateBossfightAttacks();
  }

  private void Start() {
    _currentAction = _bossIdle[0];
    _currentAction.Enter();
  }
  
  protected override void CalculateCurrentPhase() {
    switch (_bossHealth) {
      case < 1000 and > 800:
        Debug.Log("Phase 1");
        break;
      case < 800 and > 600:
        Debug.Log("Phase 2");
        break;
      case < 600 and > 300:
        Debug.Log("Phase 3");
        break;
    }
  }
  
  protected override void GenerateBossfightAttacks() {
    
    _bossAttacks = new Dictionary<int, System.Collections.Generic.List<IBossAction>>();
    _bossAttacks[0] = new List<IBossAction>();
    _bossAttacks[0].Add(new SpecterCannons());
    //_bossAttacks[0].Add(new SpecterCauldron());
    
    _bossTransitions = new Dictionary<int, IBossAction>();

    _bossIdle = new Dictionary<int, IBossAction>();
    _bossIdle[0] = new SpecterIdlePhaseOne();
  }

  public override void Move(GameObject destination, string type, float duration) {
    switch(type) {
      case "arc":
        _moveRoutine = StartCoroutine(MoveArc(destination.transform.position, duration, -3f));
        break;
    }
  }
  public override void Stop() {
    StopCoroutine(_moveRoutine);
  }
  
  private IEnumerator MoveArc(Vector3 destination, float duration, float arcHeight) {
    Vector3 startPos = transform.position;
    float elapsed = 0f;
    Vector3 midPoint = (startPos + destination) / 2 + Vector3.up * arcHeight;
    while (elapsed < duration) {
      float t = elapsed / duration;
      float easedT = easingArc.Evaluate(t);
       
      // Calculate position along a quadratic Bézier curve:
      // B(t) = (1 - t)^2 * startPos + 2 * (1 - t) * t * midPoint + t^2 * destination.     
      Vector3 pos = Mathf.Pow(1 - easedT, 2) * startPos +
                    2 * (1 - easedT) * easedT * midPoint +
                    Mathf.Pow(easedT, 2) * destination;

      transform.position = pos;
      elapsed += Time.deltaTime;
      yield return null;
    }
    //OnMoveEnd?.Invoke();
    transform.position = destination;
  }

  public void AnimEndP1Idle() {
    StopCoroutine(_moveRoutine);
    OnMoveEnd?.Invoke();
  }
  public void AnimEndP1CannonTransform() {
    ChangeAnimation("Phase1__CannonShoot");
  }
  public void AnimEndP1CannonShoot() {
    Idle();
  }

  //public enum
  public void CannonShoot() {
    GameObject sickle = Instantiate(Resources.Load<GameObject>("SpecterSickle"));
    sickle.transform.SetPositionAndRotation(transform.Find("SickleSpawnPoint").transform.position, Quaternion.Euler(0, 0, 0));
    sickle.GetComponent<SickleManager>().ExitScreen();
  }

  public void AnimEndP1PortalIn() {
    transform.position = GameObject.Find("SpecterMovePoints/Cauldron").transform.position;
    ChangeAnimation("Phase1__CauldronPortalIn");
    
    // TODO: Add LayerChange
    //GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("");
    //gameObject.layer = LayerMask.NameToLayer("Enemy");
  }
  
  public void AnimEndP1CauldronPortalIn() {
    ChangeAnimation("Phase1__CauldronIdle");
  }

  public void AnimEndP1CauldronIdle() {
    ChangeAnimation("Phase1__CauldronAttack");
  }
  public void AnimEndP1CauldronAttack() {
    ChangeAnimation("Phase1__CauldronPortalOut");
  }
  public void AnimEndP1CauldronPortalOut() {
    // TODO: Move to correct move point
    SpecterIdlePhaseOne idle = _bossIdle[0] as SpecterIdlePhaseOne;
    transform.position = idle.lastVisited.transform.position; 
    ChangeAnimation("Phase1__PortalOut");
  }
  public void AnimEndP1PortalOut() {
    Idle();
  }
  
}
