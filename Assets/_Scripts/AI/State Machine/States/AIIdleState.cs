using Enums;
using Interfaces;

namespace AI.State_Machine.States
{
    public class AIIdleState : IAIState
    {
        public void Enter(BaseAIAgent agent)
        {
        }

        public void Exit(BaseAIAgent agent)
        {
        }

        public AIState GetStateID()
        {
            return AIState.IDLE;
        }

        public void Update(BaseAIAgent agent)
        {
            if (!agent.hasTarget)
                return;
            agent.StateMachine.ChangeState(AIState.CHASE_PLAYER);
        }
    }
}
