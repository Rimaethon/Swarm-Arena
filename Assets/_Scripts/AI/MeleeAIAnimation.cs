using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class MeleeAIAnimation : MonoBehaviour, IAIMeleeAttack
{
	private int attackDamage;
	private Animator animator;
	private MeleeDamageCollider damageCollider;
	private static readonly int speed = Animator.StringToHash("Speed");
	private static readonly int lightAttack = Animator.StringToHash("LightAttack");
	private static readonly int heavyAttack = Animator.StringToHash("HeavyAttack");

	private void Start()
	{
		animator = GetComponent<Animator>();
		damageCollider = GetComponentInChildren<MeleeDamageCollider>();
	}

	private void Update()
	{
		animator.SetFloat(speed, 1, 0.2f, Time.deltaTime);
	}

	//Animation Event
	private void StartMeleeAttack()
	{
		damageCollider.EnableCollider(attackDamage);

	}

	//Animation Event
	private void EndMeleeAttack()
	{
		damageCollider.DisableCollider();

	}

	public void PerformLightMeleeAttack(BaseAIAgent agent, int damage)
	{
		agent.IsAttacking = true;
		attackDamage = damage;
		agent.Animator.SetTrigger(lightAttack);
	}

	public void PerformHeavyMeleeAttack(BaseAIAgent agent, int damage)
	{
		agent.IsAttacking = true;
		attackDamage = damage;
		agent.Animator.SetTrigger(heavyAttack);
	}
}
