using AI;
using Enums;

namespace Interfaces
{
	public interface IAIState
	{
		AIState GetStateID();
		void Enter(BaseAIAgent agent);
		void Update(BaseAIAgent agent);
		void Exit(BaseAIAgent agent);
	}
}
