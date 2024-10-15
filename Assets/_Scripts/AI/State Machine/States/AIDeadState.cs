using Enums;
using Interfaces;
using Player;

namespace AI.State_Machine.States
{
	public class AIDeadState : IAIState
	{
		public void Enter(BaseAIAgent agent)
		{
			agent.Animator.SetTrigger(AnimationHashData.Death);
		}

		public void Exit(BaseAIAgent agent)
		{
		}

		public AIState GetStateID()
		{
			return AIState.DEAD;
		}

		public void Update(BaseAIAgent agent)
		{
		}
	}
}
