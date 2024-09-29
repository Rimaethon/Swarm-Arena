using UnityEngine;

public class BaseAIAgent : PoolAbleObject
{
	public AIStateID InitialStateID;
	public AIStateID IdleStateID;
	public AIStateID ChaseStateID;
	public AIAgentConfig Config;
	public Transform player;
	public bool hasTarget;

	[HideInInspector] public AIStateMachine StateMachine;
	[HideInInspector] public Animator Animator;
	[HideInInspector] public bool IsAttacking;
	[HideInInspector] public MeleeAIAnimation meleeAIAnimation;
	[HideInInspector] public DamageAbleCharacter Status;
	private CapsuleCollider capsuleCollider;

	public override void Awake()
	{
		base.Awake();
		StateMachine = new AIStateMachine(this);
		Animator = GetComponent<Animator>();
		meleeAIAnimation = GetComponent<MeleeAIAnimation>();
		Status = GetComponent<DamageAbleCharacter>();
		capsuleCollider = GetComponent<CapsuleCollider>();
	}

	private void OnEnable()
	{
		hasTarget = true;
		capsuleCollider.enabled = true;
		if (StateMachine.currentStateID == IdleStateID||StateMachine.currentStateID == AIStateID.Dead)
			StateMachine.ChangeState(ChaseStateID);
	}

	public virtual void Start()
	{
		StateMachine.RegisterState(new AIDeadState());
	}

	public virtual void Update()
	{
		StateMachine.Update();
	}

	public void SetDeadState()
	{
		StateMachine.ChangeState(AIStateID.Dead);
		capsuleCollider.enabled = false;
		StartCoroutine(DisableOnEndCoroutine(3));
	}
}
