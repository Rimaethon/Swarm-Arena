using Enums;
using Interfaces;
using UnityEngine;

namespace AI.State_Machine.States
{
	public class AIMeleeChaseState : IAIState
	{
		public void Enter(BaseAIAgent agent)
		{
			agent.Animator.CrossFade("Blend Tree", 0.1f);
		}

		public void Exit(BaseAIAgent agent)
		{
		}

		public AIState GetStateID()
		{
			return AIState.CHASE_PLAYER;
		}

		public void Update(BaseAIAgent agent)
		{
			switch (agent.hasTarget)
			{
				case true when
					Vector3.Distance(agent.transform.position, agent.playerTransform.transform.position) <= agent.configSO.AttackDistance:
					agent.StateMachine.ChangeState(AIState.MELEE_ATTACK);
					break;
				case false:
					agent.StateMachine.ChangeState(AIState.IDLE);
					return;
			}

			if (!agent.hasTarget) return;

			agent.transform.position = Vector3.MoveTowards(agent.transform.position, agent.playerTransform.transform.position,
														   agent.configSO.Speed * Time.deltaTime);

			agent.transform.LookAt(agent.playerTransform);
		}
	}
}
