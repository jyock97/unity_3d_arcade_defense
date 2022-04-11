using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Whiteboard usage
///  - Enemy01AttackState_Agent -> expected NavMeshAgent
///  - Enemy01AttackState_Animator -> expected Animator
///  - Enemy01AttackState_Target -> expected GameObject
///  - Enemy01AttackState_FollowTime -> expected float
///  - Enemy01AttackState_KeepInitialPosition -> expected bool
/// </summary>
public class Enemy01AttackState : State
{
    public static readonly string Agent = "Enemy01AttackState_Agent";
    public static readonly string Animator = "Enemy01AttackState_Animator";
    public static readonly string Target = "Enemy01AttackState_Target";
    public static readonly string FollowTime = "Enemy01AttackState_FollowTime";
    public static readonly string KeepInitialPosition = "Enemy01AttackState_KeepInitialPosition";

    public Enemy01AttackState(StateMachine stateMachine) : base("Enemy01AttackState", stateMachine) { }

    private NavMeshAgent _agent;
    private GameObject _target;
    private float _currentFollowTime;

    public override void Start(Whiteboard data)
    {
        _agent = data.Get<NavMeshAgent>(Agent);
        _target = data.Get<GameObject>(Target);
        _agent.enabled = data.Get<bool>(KeepInitialPosition);
        _currentFollowTime = Time.time + data.Get<float>(FollowTime);
        data.Get<Animator>(Animator).SetTrigger("attack");
    }

    public override void Execute(Whiteboard data)
    {
        if (_currentFollowTime > Time.time)
        {
            Quaternion q = Quaternion.LookRotation((_target.transform.position - _stateMachine.transform.position).normalized);
            _stateMachine.transform.rotation = Quaternion.Euler(new Vector3(0, q.eulerAngles.y, 0));
        }
    }

    public override void Exit(Whiteboard data)
    {
        _agent.enabled = true;
        _agent.SetDestination(_stateMachine.transform.position);
    }
}