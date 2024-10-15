using AI.State_Machine.States;
using Enums;
using Interfaces;

namespace AI
{
	public class MeleeAIAgent : BaseAIAgent
	{
		public IAIMeleeAttack AIAttack { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			AIAttack = GetComponent<IAIMeleeAttack>();
		}

		protected override void Start()
		{
			base.Start();
			StateMachine.RegisterState(new AIMeleeChaseState());
			StateMachine.RegisterState(new AIIdleState());
			StateMachine.RegisterState(new AIMeleeAttackState());
			StateMachine.ChangeState(AIState.IDLE);
		}
	}
}
