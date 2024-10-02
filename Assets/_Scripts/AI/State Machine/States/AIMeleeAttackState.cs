using UnityEngine;

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

    public AIStateID GetStateID()
    {
        return AIStateID.MeleeAttack;
    }

    public void Update(BaseAIAgent agent)
    {
        if (agent.IsAttacking)
        {
            return;
        }

        if (!agent.hasTarget)
        {
            agent.StateMachine.ChangeState(AIStateID.Idle);

            return;
        }

        if (!agent.player.TryGetComponent<IDamageAble>(out var status) || status.IsDead)
        {
            agent.player=null;
            agent.hasTarget=false;
            agent.StateMachine.ChangeState(AIStateID.Idle);

            return;
        }

        if (agent.hasTarget && Vector3.Distance(agent.transform.position, agent.player.transform.position) > agent.Config.AttackDistance)
        {
            agent.StateMachine.ChangeState(AIStateID.ChasePlayer);
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
        agent.transform.LookAt(agent.player.transform.position, Vector3.up);
        MeleeAIAgent meleeAgent = agent as MeleeAIAgent;
        meleeAgent.AIAttack.PerformLightMeleeAttack(agent, agent.Config.LightDamage);
    }

    private void HeavyAttack(BaseAIAgent agent)
    {
        agent.transform.LookAt(agent.player.transform.position, Vector3.up);
        MeleeAIAgent meleeAgent = agent as MeleeAIAgent;
        meleeAgent.AIAttack.PerformHeavyMeleeAttack(agent, agent.Config.HeavyDamage);
    }
}
