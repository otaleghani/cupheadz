using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public enum BossState
{
  Attack, // Use current attack, show intro animation
  Idle,
  Enter, // Show entry animation
  PhaseTransition,
  Dead, // The character is dying, show exit animation
}

public abstract class BossStateManager : MonoBehaviour, IDamageable
{
  public static BossStateManager Instance;

  public event Action OnDamage;

  public bool isFacingRight = false;
  public bool isPlayerDead;
  protected IBossAction _currentAction;
  protected float _bossHealth;
  protected float _bossCurrentHealth;
  protected int _bossPhase = 0;
  public int BossPhase => _bossPhase;
  protected BossState _state = BossState.Enter;
  protected Animator _animator;
  protected Dictionary<int, List<IBossAction>> _bossAttacks;
  protected Dictionary<int, IBossAction> _bossTransitions;
  protected Dictionary<int, IBossAction> _bossIdle;
  protected float _minTimeToAttack;
  protected float _maxTimeToAttack;
  protected IBossAction _currentAttack;
  private float _counter;
  private float GenerateTimeToNextAttack()
  {
    return UnityEngine.Random.Range(_minTimeToAttack, _maxTimeToAttack);
  }

  protected List<int> attackPattern;

  public float Health
  {
    get { return _bossHealth; }
    private set { _bossHealth = value; }
  }
  public float CurrentHealth
  {
    get { return _bossCurrentHealth; }
    private set { _bossCurrentHealth = value; }
  }
  //public float TotalHealth {}

  protected virtual void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Debug.LogWarning("Found more than one BossStateManager in the scene.");
    }
    _animator = GetComponent<Animator>();
  }
  protected virtual void Start() { }

  private void FixedUpdate()
  {
    _currentAction.Update();
    //if (_counter <= 0 && _state == BossState.Idle) {
    //Attack();
    //}
    //if (_state == BossState.Attack) {
    //_currentAction.Update();
    //}
  }

  public void ChangeAnimation(string animation)
  {
    _animator.Play(animation);
  }

  /// <summary>Based on the current boss health, call if needed te ChangePhase</summary>
  protected abstract void CalculateCurrentPhase();
  /// <summary>Populate the Dictionary _bossAttacks</summary>
  protected abstract void GenerateBossfightAttacks();

  protected void ChangePhase(int phaseNumber)
  {
    _bossPhase = phaseNumber;
  }

  protected void ChangeBossAction(IBossAction action)
  {
    _currentAction.Exit();
    _currentAction = action;
    _currentAction.Enter();
  }

  protected void CreatePhasePattern()
  {
    int maxAttacks = _bossAttacks[_bossPhase].Count;
    attackPattern = new List<int>();

    if (maxAttacks == 1)
    {
      for (int i = 0; i < 10; i++)
      {
        attackPattern.Add(0);
      }
      return;
    }
    attackPattern.Add(UnityEngine.Random.Range(0, maxAttacks));
    while (attackPattern.Count < 10)
    {
      int newIndex = UnityEngine.Random.Range(0, maxAttacks);

      while (attackPattern.Last() == newIndex)
      {
        newIndex = UnityEngine.Random.Range(0, maxAttacks);
      }
      attackPattern.Add(newIndex);
    }

    foreach (int i in attackPattern)
    {
      Debug.Log(i);
    }
  }

  /// Based on the current phase, get one attack randomly
  private int currentIndex = 0;
  public void Attack()
  {
    ChangeBossAction(_bossAttacks[_bossPhase][attackPattern[currentIndex]]);
    currentIndex = (currentIndex + 1) % attackPattern.Count;
    // IBossAction newAction;
    // if (_bossAttacks[_bossPhase].Count > 1) {
    // 	do {
    // 		newAction = _bossAttacks[_bossPhase][UnityEngine.Random.Range(0, _bossAttacks[_bossPhase].Count)];
    // 	} while (newAction == _currentAction);
    // 	ChangeBossAction(newAction);
    // } else {
    // 	ChangeBossAction(_bossAttacks[_bossPhase][0]);
    // }
    _state = BossState.Attack;
  }

  /// Based on the current phase, get the idle action
  public virtual void Idle()
  {
    if (_state == BossState.PhaseTransition) return;
    ChangeBossAction(_bossIdle[_bossPhase]);
    _state = BossState.Idle;
  }

  /// Based on the current phase, get the transition action
  public void Transition()
  {
    Debug.Log("Called Transition");
    ChangeBossAction(_bossTransitions[_bossPhase]);
    _state = BossState.PhaseTransition;
  }

  /// Used to reset the counter
  public void SetTimer(float time)
  {
    _counter = time;
  }

  public void TakeDamage(float amount)
  {
    _bossCurrentHealth -= amount;
    //CalculateCurrentPhase();
    if (_bossCurrentHealth <= 0)
    {
      Die();
    }
    OnDamage?.Invoke();
  }
  protected void Die()
  {
    Debug.Log("The boss died!");

    FightSceneStateManager.Instance.ChangeState(FightSceneStateManager.SceneState.Lose);
  }

  public virtual void Move(GameObject destination, string type, float duration) { }
  public virtual void Stop() { }

  public virtual void DeathCard()
  {
    isPlayerDead = true;
  }

  protected void MakeInvulnerable() { }

  public void DisableCollider()
  {
    GetComponent<BoxCollider2D>().enabled = false;
  }

  public void EnableCollider()
  {
    GetComponent<BoxCollider2D>().enabled = true;
  }

  public void HandleFlip()
  {
    isFacingRight = !isFacingRight;
    Vector3 ls = transform.localScale;
    ls.x *= -1f;
    transform.localScale = ls;
  }
}
