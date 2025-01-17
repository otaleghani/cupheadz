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
	
	protected float _bossHealth;
	protected int _bossPhase = 0;
	protected BossState _state = BossState.Enter;
	protected Animator _animator;
	protected Dictionary<int, List<IBossAttack>> _bossStages;
	protected float _minTimeToAttack;
	protected float _maxTimeToAttack;
	private float _counter;
	private IBossAttack _currentAttack;
	
	private float GenerateTimeToNextAttack() {
		return Random.Range(_minTimeToAttack, _maxTimeToAttack);
	}

	private void FixedUpdate() {
		if (_counter <= 0 && _state == BossState.Idle) {
			// Todo: queue new attack
		}
		if (_state == BossState.Attack) {
			_currentAttack.Update();
		}
	}

	/// <summary>Based on the current boss health, call if needed te ChangePhase</summary>
	protected abstract void CalculateCurrentPhase();
	/// <summary>Populate the Dictionary _bossStages</summary>
	protected abstract void GenerateBossfightAttacks();
	
	private void Start() {
		if (Instance == null) {
			Instance = this;
		} else {
			Debug.LogWarning("Found more than one BossStateManager in the scene.");
		}
	}

	protected void ChangePhase() {
		_bossPhase += 1;	
	}

	protected void SetNewAttack() {
		_currentAttack = _bossStages[_bossPhase][Random.Range(0, _bossStages[_bossPhase].Count)];
		_state = BossState.Attack;
	}
		
	public void TakeDamage(float amount) {
		_bossHealth -= amount;
		if (_bossHealth <= 0) {
			CalculateCurrentPhase();
			Die();
		}
	}
	protected void Die() {
		Debug.Log("The boss died!");
	}

	protected void MakeInvulnerable() { }
	//protected void
}
