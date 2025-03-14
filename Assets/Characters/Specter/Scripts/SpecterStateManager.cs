using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpecterStateManager : BossStateManager
{
  public static event Action OnMoveEnd;
  private Coroutine _moveRoutine;
  private AnimationCurve easingArc = AnimationCurve.EaseInOut(0, 0, 1, 1);

  private float _firstPhaseHealth = 3000;
  private float _secondPhaseHealth = 2500;
  private float _thirdPhaseHealth = 1000;

  protected override void Awake()
  {
    base.Awake();
    _bossHealth = 3000;
    _bossCurrentHealth = _bossHealth;
    GenerateBossfightAttacks();
    FightSceneStateManager.Instance.CurrentScene = FightSceneStateManager.SceneName.Specter;
  }

  protected override void Start()
  {
    base.Start();
    _currentAction = _bossTransitions[0];
    _currentAction.Enter();
    _attackType = UnityEngine.Random.Range(0, 2);

    // AudioManager.Instance.Play("Soundtrack");
    AudioManager.Instance.Play("Static");
    // AudioManager.Instance.Play("IntroPt1");
  }

  protected override void CalculateCurrentPhase()
  {
    if (CurrentHealth < _firstPhaseHealth && CurrentHealth > _secondPhaseHealth)
    {
      Debug.Log("Phase 1");
      return;
    }
    if (CurrentHealth < _secondPhaseHealth && CurrentHealth > _thirdPhaseHealth)
    {
      if (_bossPhase != 1)
      {
        ChangePhase(1);
        Transition();
      }
      Debug.Log("Phase 2");
      return;
    }
    if (CurrentHealth < _thirdPhaseHealth && CurrentHealth > 0)
    {
      if (_bossPhase != 2)
      {
        Transition();
        ChangePhase(2);
      }
      Debug.Log("Phase 3");
      return;
    }
  }

  protected override void GenerateBossfightAttacks()
  {
    _bossAttacks = new Dictionary<int, System.Collections.Generic.List<IBossAction>>();
    _bossAttacks[0] = new List<IBossAction>();
    _bossAttacks[0].Add(new SpecterCannons());
    _bossAttacks[0].Add(new SpecterClock());
    _bossAttacks[0].Add(new SpecterCauldron());
    CreatePhasePattern();

    _bossTransitions = new Dictionary<int, IBossAction>();
    _bossTransitions[0] = GetComponent<SpecterTransitionPhaseOne>();
    _bossTransitions[1] = GetComponent<SpecterTransitionPhaseTwo>();

    _bossIdle = new Dictionary<int, IBossAction>();
    _bossIdle[0] = GetComponent<SpecterIdlePhaseOne>();
    _bossIdle[1] = GetComponent<SpecterIdlePhaseOne>();
  }

  public override void Move(GameObject destination, string type, float duration)
  {
    switch (type)
    {
      case "arc":
        _moveRoutine = StartCoroutine(MoveArc(destination.transform.position, duration, -3f));
        break;
    }
  }
  public override void Stop()
  {
    StopCoroutine(_moveRoutine);
  }

  private IEnumerator MoveArc(Vector3 destination, float duration, float arcHeight)
  {
    Vector3 startPos = transform.position;
    float elapsed = 0f;
    Vector3 midPoint = (startPos + destination) / 2 + Vector3.up * arcHeight;
    while (elapsed < duration)
    {
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

  public override void Idle()
  {
    base.Idle();
    SpecterAudioManager.Instance.PlayIdleSound();
  }

  // This is the intro
  public void AnimEndP1Transition()
  {
    Idle();
  }
  public void AnimEndP1Idle()
  {
    CalculateCurrentPhase();
    if (_bossPhase == 0)
    {
      StopCoroutine(_moveRoutine);
      OnMoveEnd?.Invoke();
    }
    else
    {
      /*SpecterTransitionPhaseTwo.Instance.CanTransition = true;*/
      SpecterTransitionPhaseTwo.Instance.DoTransition();
    }
  }
  public void AnimEndP1CannonTransform()
  {
    if (UnityEngine.Random.Range(0, 2) == 1)
    {
      ChangeAnimation("Phase1__CannonShootVariant");
    }
    else
    {
      ChangeAnimation("Phase1__CannonShoot");
    }
  }
  public void AnimEndP1CannonShoot()
  {
    ChangeAnimation("Phase1__CannonTransformOut");
  }
  public void AnimEndP1CannonTransformOut()
  {
    Idle();
    CannonShootReset();
  }



  private int _sickleCounter = 1;
  private int _attackType;
  public void CannonShoot()
  {
    // Here add the gameobject
    // randomly choose to create a parriable sickle

    GameObject sickle = Instantiate(Resources.Load<GameObject>("SpecterSickle"));
    sickle.transform.SetPositionAndRotation(transform.Find("SickleSpawnPoint").transform.position, Quaternion.Euler(0, 0, 0));
    SpecterIdlePhaseOne idle = _bossIdle[0] as SpecterIdlePhaseOne;
    sickle.GetComponent<SickleManager>().StartSickle(idle.lastVisited.name == "IdleMoveLeft", _sickleCounter, _attackType);
    _sickleCounter++;
  }
  public void CannonShootReset()
  {
    _sickleCounter = 1;
    _attackType = UnityEngine.Random.Range(0, 2);

  }

  public void AnimEndP1PortalIn()
  {
    transform.position = GameObject.Find("SpecterMovePoints/Cauldron").transform.position;
    ChangeAnimation("Phase1__CauldronPortalIn");

    // TODO: Add LayerChange
    //GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("");
    //gameObject.layer = LayerMask.NameToLayer("Enemy");
  }

  public void AnimEndP1CauldronPortalIn()
  {
    ChangeAnimation("Phase1__CauldronIdle");
  }

  public void AnimEndP1CauldronIdle()
  {
    ChangeAnimation("Phase1__CauldronAttack");
  }
  public void AnimEndP1CauldronAttack()
  {
    ChangeAnimation("Phase1__CauldronPortalOut");
    // Here spawn
    CauldronSpawnerManager.Instance.SpawnBullets();
  }
  public void AnimEndP1CauldronPortalOut()
  {
    // TODO: Move to correct move point
    SpecterIdlePhaseOne idle = _bossIdle[0] as SpecterIdlePhaseOne;
    transform.position = idle.lastVisited.transform.position;
    ChangeAnimation("Phase1__PortalOut");
  }
  public void AnimEndP1PortalOut()
  {
    Idle();
  }

  public void SpriteCauldronLayer()
  {
    GetComponent<SpriteRenderer>().sortingLayerName = "Background";
    GetComponent<SpriteRenderer>().sortingOrder = 3;
    gameObject.tag = "EnemyUnreachable";
    gameObject.layer = LayerMask.NameToLayer("EnemyUnreachable");
  }
  public void SpriteSpecterLayer()
  {
    GetComponent<SpriteRenderer>().sortingLayerName = "Boss";
    GetComponent<SpriteRenderer>().sortingOrder = 0;
    gameObject.tag = "Enemy";
    gameObject.layer = LayerMask.NameToLayer("Enemy");
  }
  public void SpriteMantisLayer()
  {
    GetComponent<SpriteRenderer>().sortingLayerName = "Background";
    GetComponent<SpriteRenderer>().sortingOrder = 5;
    gameObject.tag = "EnemyUnreachable";
    gameObject.layer = LayerMask.NameToLayer("EnemyUnreachable");
  }

  public void AnimEndP1Clock()
  {
    Idle();
    // Spawn either left or right the shit
    if (isFacingRight)
    {
      GameObject obj = Instantiate(Resources.Load<GameObject>("SkeletonSquare"));
      Vector3 ls = obj.transform.localScale;
      ls.x *= -1f;
      obj.transform.localScale = ls;
    }
    else
    {
      Instantiate(Resources.Load<GameObject>("SkeletonSquare"));
    }
  }
}
