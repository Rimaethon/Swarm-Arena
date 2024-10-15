using AI.State_Machine;
using AI.State_Machine.States;
using Enums;
using Object_Pool;
using Scriptable_Objects;
using UnityEngine;

namespace AI
{
	public class BaseAIAgent : PoolAbleObject
	{
		public AIAgentConfigSO configSO;
		public Transform playerTransform;
		public bool hasTarget;
		[HideInInspector]
		public Animator Animator;
		[HideInInspector]
		public bool IsAttacking;
		private CapsuleCollider capsuleCollider;
		[HideInInspector]
		public AIStateMachine StateMachine;

		protected override void Awake()
		{
			base.Awake();
			StateMachine = new AIStateMachine(this);
			Animator = GetComponent<Animator>();
			capsuleCollider = GetComponent<CapsuleCollider>();
		}

		protected virtual void Start()
		{
			StateMachine.RegisterState(new AIDeadState());
		}

		protected virtual void Update()
		{
			StateMachine.Update();
		}

		private void OnEnable()
		{
			hasTarget = true;
			capsuleCollider.enabled = true;
			StateMachine.ChangeState(AIState.CHASE_PLAYER);
		}

		public void SetDeadState()
		{
			StateMachine.ChangeState(AIState.DEAD);
			capsuleCollider.enabled = false;
			StartCoroutine(DisableOnEndCoroutine(3));
		}
	}
}
