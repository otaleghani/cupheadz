using UnityEngine;
using System.Collections.Generic;

public enum BossState {
	Attack, // Use current attack, show intro animation
	Idle,
	Enter, // Show entry animation
	PhaseTransition,
	Dead, // The character is dying, show exit animation
}

public abstract class BossStateManager : MonoBehaviour, IDamageable {
	public static BossStateManager Instance;
	
	public bool isFacingRight = false;
	protected IBossAction _currentAction;
	protected float _bossHealth;
	protected int _bossPhase = 0;
	protected BossState _state = BossState.Enter;
	protected Animator _animator;
	protected Dictionary<int, List<IBossAction>> _bossAttacks;
	protected Dictionary<int, IBossAction> _bossTransitions;
	protected Dictionary<int, IBossAction> _bossIdle;
	protected float _minTimeToAttack;
	protected float _maxTimeToAttack;
	private float _counter;
	private float GenerateTimeToNextAttack() {
		return UnityEngine.Random.Range(_minTimeToAttack, _maxTimeToAttack);
	}

	protected virtual void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Debug.LogWarning("Found more than one BossStateManager in the scene.");
		}
		_animator = GetComponent<Animator>();
	}

	private void FixedUpdate() {
		Debug.Log(_currentAction);
		_currentAction.Update();
		//if (_counter <= 0 && _state == BossState.Idle) {
			//Attack();
		//}
		//if (_state == BossState.Attack) {
			//_currentAction.Update();
		//}
	}

	public void ChangeAnimation(string animation) {
		_animator.Play(animation);
	}

	/// <summary>Based on the current boss health, call if needed te ChangePhase</summary>
	protected abstract void CalculateCurrentPhase();
	/// <summary>Populate the Dictionary _bossAttacks</summary>
	protected abstract void GenerateBossfightAttacks();

	protected void ChangePhase() {
		_bossPhase += 1;	
	}

	protected void ChangeBossAction(IBossAction action) {
		_currentAction.Exit();
		_currentAction = action;
		_currentAction.Enter();
	}
	
	/// Based on the current phase, get one attack randomly
	public void Attack() {
		Debug.Log("The count is: " + _bossAttacks [_bossPhase].Count);
		ChangeBossAction(_bossAttacks[_bossPhase][UnityEngine.Random.Range(0, _bossAttacks[_bossPhase].Count)]);
		_state = BossState.Attack;
	}

	/// Based on the current phase, get the idle action
	public void Idle() {
		ChangeBossAction(_bossIdle[_bossPhase]);
		_state = BossState.Idle;
	}

	/// Based on the current phase, get the transition action
	public void Transition() {
		ChangeBossAction(_bossTransitions[_bossPhase]);
		_state = BossState.PhaseTransition;
	}
	
	/// Used to reset the counter
	public void SetTimer(float time) {
		_counter = time;
	}
		
	public void TakeDamage(float amount) {
		_bossHealth -= amount;
		CalculateCurrentPhase();
		if (_bossHealth <= 0) {
			Die();
		}
	}
	protected void Die() {
		Debug.Log("The boss died!");
	}

	public virtual void Move(GameObject destination, string type, float duration) {}
	public virtual void Stop() {}

	protected void MakeInvulnerable() {}
	
	public void HandleFlip() {
    isFacingRight = !isFacingRight;
    Vector3 ls = transform.localScale;
    ls.x *= -1f;
    transform.localScale = ls;
	}

	// todo: OnTriggerEnter2D take damage
}
