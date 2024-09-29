using UnityEngine;

public class AIIdleState : IAIState
{
    public void Enter(BaseAIAgent agent)
    {
    }

    public void Exit(BaseAIAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Idle;
    }

    public void Update(BaseAIAgent agent)
    {
        if (!agent.hasTarget) return;
        agent.StateMachine.ChangeState(AIStateID.ChasePlayer);
    }
}
