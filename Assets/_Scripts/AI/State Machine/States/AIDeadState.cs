
using UnityEngine;

public class AIDeadState : IAIState
{
    private static readonly int dead = Animator.StringToHash("Dead");

    public void Enter(BaseAIAgent agent)
    {
        agent.Animator.SetTrigger(dead);
    }

    public void Exit(BaseAIAgent agent)
    {
    }

    public AIStateID GetStateID()
    {
        return AIStateID.Dead;
    }

    public void Update(BaseAIAgent agent)
    {
    }
}
