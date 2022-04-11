using System;

public abstract class State
{
    public readonly String stateName;
    protected readonly StateMachine _stateMachine;

    protected State(String name, StateMachine stateMachine)
    {
        stateName = name;
        _stateMachine = stateMachine;
    }
    
    public abstract void Start(Whiteboard data);
    public abstract void Execute(Whiteboard data);
    public abstract void Exit(Whiteboard data);
}
