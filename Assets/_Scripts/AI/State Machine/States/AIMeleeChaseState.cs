using UnityEngine;

public class AIMeleeChaseState : IAIState
{
	public void Enter(BaseAIAgent agent)
	{
	//	agent.NavMeshAgent.isStopped = false;
		agent.Animator.CrossFade("Blend Tree", 0.1f);
	}

	public void Exit(BaseAIAgent agent)
	{
	}

	public AIStateID GetStateID()
	{
		return AIStateID.ChasePlayer;
	}

	public void Update(BaseAIAgent agent)
	{
		switch (agent.hasTarget)
		{
			case true when
				Vector3.Distance(agent.transform.position, agent.player.transform.position) <= agent.Config.AttackDistance:
				agent.StateMachine.ChangeState(AIStateID.MeleeAttack);
				break;
			case false:
				agent.StateMachine.ChangeState(AIStateID.Idle);
				return;
		}

		if (agent.hasTarget)
		{
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, agent.player.transform.position,
				agent.Config.Speed * Time.deltaTime);
			agent.transform.LookAt(agent.player);
		}
	}
}
