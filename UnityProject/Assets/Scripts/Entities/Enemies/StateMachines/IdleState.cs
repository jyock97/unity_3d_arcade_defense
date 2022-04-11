/// <summary>
/// </summary>
public class IdleState: State
{
    public IdleState(StateMachine stateMachine) : base("IdleState", stateMachine) { }

    public override void Start(Whiteboard data) { }

    public override void Execute(Whiteboard data) { }

    public override void Exit(Whiteboard data) { }
}