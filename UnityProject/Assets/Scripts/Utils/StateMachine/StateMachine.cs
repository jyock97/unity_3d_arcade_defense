using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    [SerializeField] protected Boolean debugState;
    
    [HideInInspector] public EntityController _entityController;
    
    protected State _currentState;
    protected Whiteboard _data;

    
    protected virtual void OnValidate() { }

    protected virtual void Start()
    {
        _entityController = GetComponent<EntityController>();
        _data = new Whiteboard();
        InitWhiteBoard();
        
        _currentState.Start(_data);
        if (debugState)
        {
            Debug.Log($"Start: {_currentState.stateName}");
        }
    }

    protected virtual void Update()
    {
        _currentState.Execute(_data);
        if (debugState)
        {
            Debug.Log($"Execute: {_currentState.stateName}");
        }
    }

    protected abstract void InitWhiteBoard();

    public void Exit()
    {
        _currentState.Exit(_data);
        if (debugState)
        {
            Debug.Log($"Exit: {_currentState.stateName}");
        }
    }
    
    public void ChangeState(State state)
    {
        
        _currentState.Exit(_data);
        if (debugState)
        {
            Debug.Log($"Exit: {_currentState.stateName}");
        }
        _currentState = state;
        _currentState.Start(_data);
        if (debugState)
        {
            Debug.Log($"Start: {_currentState.stateName}");
        }
    }
}
