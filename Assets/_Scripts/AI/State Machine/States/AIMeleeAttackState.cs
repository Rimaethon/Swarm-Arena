using Enums;
using Interfaces;
using UnityEngine;

namespace AI.State_Machine.States
{
	public class AIMeleeAttackState : IAIState
	{
		public void Enter(BaseAIAgent agent)
		{
		}

		public void Exit(BaseAIAgent agent)
		{
			agent.IsAttacking = false;
			agent.Animator.CrossFade("Blend Tree", 0.1f);
		}

		public AIState GetStateID()
		{
			return AIState.MELEE_ATTACK;
		}

		public void Update(BaseAIAgent agent)
		{
			if (agent.IsAttacking)
			{
				return;
			}

			if (!agent.hasTarget)
			{
				agent.StateMachine.ChangeState(AIState.IDLE);
				return;
			}

			if (!agent.playerTransform.TryGetComponent(out IDamageAble status) || status.IsDead)
			{
				agent.playerTransform = null;
				agent.hasTarget = false;
				agent.StateMachine.ChangeState(AIState.IDLE);
				return;
			}

			if (agent.hasTarget && Vector3.Distance(agent.transform.position, agent.playerTransform.transform.position) > agent.configSO.AttackDistance)
			{
				agent.StateMachine.ChangeState(AIState.CHASE_PLAYER);
			}
			else
			{
				PerformAttack(agent);
			}
		}

		private void PerformAttack(BaseAIAgent agent)
		{
			if (agent.IsAttacking) return;
			int a = Random.Range(0, 101);

			if (a >= 50)
			{
				LightAttack(agent);
			}
			else
			{
				HeavyAttack(agent);
			}
		}

		private void LightAttack(BaseAIAgent agent)
		{
			agent.transform.LookAt(agent.playerTransform.transform.position, Vector3.up);
			MeleeAIAgent meleeAgent = agent as MeleeAIAgent;
			meleeAgent.AIAttack.PerformLightMeleeAttack(agent, agent.configSO.LightDamage);
		}

		private void HeavyAttack(BaseAIAgent agent)
		{
			agent.transform.LookAt(agent.playerTransform.transform.position, Vector3.up);
			MeleeAIAgent meleeAgent = agent as MeleeAIAgent;
			meleeAgent.AIAttack.PerformHeavyMeleeAttack(agent, agent.configSO.HeavyDamage);
		}
	}
}
