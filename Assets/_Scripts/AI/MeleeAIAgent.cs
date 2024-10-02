
public class MeleeAIAgent : BaseAIAgent
{
    public IAIMeleeAttack AIAttack { get; private set; }

    public override void Awake()
    {
        base.Awake();

        AIAttack = GetComponent<IAIMeleeAttack>();
    }

    public override void Start()
    {
        base.Start();
        StateMachine.RegisterState(new AIMeleeChaseState());
        StateMachine.RegisterState(new AIIdleState());
        StateMachine.RegisterState(new AIMeleeAttackState());
        StateMachine.ChangeState(InitialStateID);
    }

    public void SetAttackToFalse()
    {
        IsAttacking = false;
    }
}
