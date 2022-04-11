using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Whiteboard usage
///  - MoveToTarget_Agent -> expected NavMeshAgent
///  - MoveToTarget_Animator -> expected Animator
///  - MoveToTarget_Animator_MovingFlag -> expected String
///  - MoveToTarget_Target -> expected GameObject
///  - MoveToTarget_FollowTarget -> expected Boolean
///  - MoveToTarget_Speed -> expected float
///  - MoveToTarget_DistanceOffset -> expected float
/// </summary>
public class MoveToTargetState : State
{
    public static readonly string Agent = "MoveToTarget_Agent";
    public static readonly string Animator = "MoveToTarget_Animator";
    public static readonly string AnimatorMovingFlag = "MoveToTarget_Animator_MovingFlag";
    public static readonly string Target = "MoveToTarget_Target";
    public static readonly string FollowTarget = "MoveToTarget_FollowTarget";
    public static readonly string Speed = "MoveToTarget_Speed";
    public static readonly string DistanceOffset = "MoveToTarget_DistanceOffset";

    private MoveToTargetState(StateMachine stateMachine): base("MoveToTargetState", stateMachine) { }
    private static State _instance;
    public static State Instance(StateMachine stateMachine)
    {
        return _instance ?? (_instance = new MoveToTargetState(stateMachine));
    }
    

    private NavMeshAgent _agent;
    
    public override void Start(Whiteboard data)
    {
        MoveToTarget(data);
        data.Get<Animator>(Animator).SetBool(data.Get<String>(AnimatorMovingFlag), true);
    }

    public override void Execute(Whiteboard data)
    {
        if (data.Get<Boolean>(FollowTarget) && false)
        {
            MoveToTarget(data);
        }
    }

    public override void Exit(Whiteboard data)
    {
        data.Get<Animator>(Animator).SetBool(data.Get<String>(AnimatorMovingFlag), false);
    }
    
    private void MoveToTarget(Whiteboard data)
    {
        GameObject target = data.Get<GameObject>(Target);
        Vector3 direction = (target.transform.position - _stateMachine.transform.position).normalized;
        float distanceOffset = data.Get<float>(DistanceOffset);
        _agent = data.Get<NavMeshAgent>(Agent);
        
        _agent.SetDestination(target.transform.position - (direction * distanceOffset));
        _agent.speed = data.Get<float>(Speed);
    }
}