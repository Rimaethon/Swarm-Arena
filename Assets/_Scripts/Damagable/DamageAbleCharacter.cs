using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BaseAIAgent))]
public class DamageAbleCharacter : MonoBehaviour, IDamageAble
{
	[SerializeField] protected float _maxHP = 100f;
	[SerializeField] protected float _hp = 100f;
	public UnityAction<GameObject> OnTakeDamageEvent;
	protected BaseAIAgent _agent;
	protected bool _isDead;
	public bool IsDead => _isDead;

	protected virtual void Awake()
	{
		_agent = GetComponent<BaseAIAgent>();
	}

	private void OnEnable()
	{
		_hp = _maxHP;
		_isDead = false;
	}

	public void TakeDamage(int damage, GameObject targetCausedDamage)
	{
		if (_isDead)
		{
			return;
		}
		CalculateDamage(damage);
		if (!_isDead)
		{
			OnTakeDamageEvent?.Invoke(targetCausedDamage);
		}
	}

	protected virtual void CalculateDamage(int damage)
	{
		_hp -= damage;
		_hp = Mathf.Clamp(_hp, 0, _maxHP);
		if (!(_hp <= 0)) return;
		_isDead = true;
		_agent.SetDeadState();
	}
}
