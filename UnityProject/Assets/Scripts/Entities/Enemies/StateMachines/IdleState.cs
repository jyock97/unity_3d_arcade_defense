/// <summary>
/// </summary>
public class IdleState: State
{
    private IdleState(StateMachine stateMachine) : base("IdleState", stateMachine) { }
    private static State _instance;
    public static State Instance(StateMachine stateMachine)
    {
        return _instance ?? (_instance = new IdleState(stateMachine));
    }

    public override void Start(Whiteboard data) { }

    public override void Execute(Whiteboard data) { }

    public override void Exit(Whiteboard data) { }
}