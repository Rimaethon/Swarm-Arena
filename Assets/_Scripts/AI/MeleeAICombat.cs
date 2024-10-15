using Data;
using Enums;
using Interfaces;
using Player;
using UnityEngine;

namespace AI
{
	[RequireComponent(typeof(Animator))]
	public class MeleeAICombat : MonoBehaviour, IAIMeleeAttack
	{
		[SerializeField]
		private Collider damageCollider;
		private Animator animator;
		private int attackDamage;

		private void Start()
		{
			animator = GetComponent<Animator>();
			damageCollider = GetComponentInChildren<SphereCollider>();
			damageCollider.enabled = false;
			damageCollider.isTrigger = true;
		}

		private void Update()
		{
			animator.SetFloat(AnimationHashData.Speed, 1, 0.2f, Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (damageCollider.enabled == false)
			{
				return;
			}

			if (!other.transform.root.gameObject.TryGetComponent(out IDamageAble damageAble))
			{
				return;
			}

			damageAble.TakeDamage(attackDamage);

			ImpactData impactData = new ImpactData
			{
				ImpactType = ImpactType.KNOCK_BACK,
				ImpactStrength = attackDamage,
				HitNormal = Vector3.forward,
				HitPoint = other.transform.root.gameObject.transform.position + Vector3.up
			};

			damageAble.HandleImpact(impactData);
		}

		public void PerformLightMeleeAttack(BaseAIAgent agent, int damage)
		{
			agent.IsAttacking = true;
			attackDamage = damage;
			agent.Animator.SetTrigger(AnimationHashData.LightAttack);
		}

		public void PerformHeavyMeleeAttack(BaseAIAgent agent, int damage)
		{
			agent.IsAttacking = true;
			attackDamage = damage;
			agent.Animator.SetTrigger(AnimationHashData.HeavyAttack);
		}

		//Animation Event
		private void StartMeleeAttack()
		{
			damageCollider.enabled = true;
		}

		//Animation Event
		private void EndMeleeAttack()
		{
			damageCollider.enabled = false;
		}
	}
}
