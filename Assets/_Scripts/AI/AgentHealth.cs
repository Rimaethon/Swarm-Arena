using AI;
using Data;
using Enums;
using Event_System;
using Interfaces;
using Object_Pool;
using UnityEngine;

namespace DamageAble
{
	[RequireComponent(typeof(BaseAIAgent))]
	public class AgentHealth : MonoBehaviour, IDamageAble
	{
		[SerializeField]
		protected float characterHealth = 100f;
		private readonly EnemyDamagedEventArgs enemyDamagedEventArgs = new EnemyDamagedEventArgs();
		private BaseAIAgent agent;

		protected virtual void Awake()
		{
			agent = GetComponent<BaseAIAgent>();
		}

		private void OnEnable()
		{
			characterHealth = agent.configSO.Health;
		}

		public Vector3 Position => transform.position;
		public bool IsDead => characterHealth <= 0;

		public void TakeDamage(int damage)
		{
			if (IsDead)
			{
				return;
			}

			characterHealth -= damage;

			//Needs pooling since it generates a lot of garbage and it is called multiple times every frame
			enemyDamagedEventArgs.EnemyType = EnemyType.NORMAL_ZOMBIE;
			enemyDamagedEventArgs.Damage = damage;
			enemyDamagedEventArgs.Position = transform.position;
			enemyDamagedEventArgs.isDead = IsDead;
			EventManager.RaiseEvent(enemyDamagedEventArgs);

			if (IsDead)
			{
				agent.SetDeadState();
			}
		}

		public void HandleImpact(ImpactData impactData)
		{
			ObjectPool particlePool = ObjectPool.CreateInstance(agent.configSO.impactSOData.EffectPrefab, 10);
			PoolAbleObject instance = particlePool.GetObject(impactData.HitPoint + impactData.HitNormal * 0.001f, Quaternion.LookRotation(impactData.HitNormal));
			instance.transform.forward = impactData.HitNormal;
		}
	}
}
