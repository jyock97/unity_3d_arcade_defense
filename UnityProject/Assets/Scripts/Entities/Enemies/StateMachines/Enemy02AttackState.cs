using UnityEngine;

/// <summary>
/// Whiteboard usage
///  - Enemy02AttackState_Agent -> expected NavMeshAgent
///  - Enemy02AttackState_Animator -> expected Animator
///  - Enemy02AttackState_Target -> expected GameObject
///  - Enemy02AttackState_FollowTime -> expected float
///  - Enemy02AttackState_KeepInitialPosition -> expected bool
/// </summary>
public class Enemy02AttackState : State
{
    public static readonly string Agent = "Enemy02AttackState_Agent";
    public static readonly string Animator = "Enemy02AttackState_Animator";
    public static readonly string Target = "Enemy02AttackState_Target";
    public static readonly string FollowTime = "Enemy02AttackState_FollowTime";
    public static readonly string KeepInitialPosition = "Enemy02AttackState_KeepInitialPosition";

    public Enemy02AttackState(StateMachine stateMachine) : base("Enemy02AttackState", stateMachine) { }

    private UnityEngine.AI.NavMeshAgent _agent;
    private GameObject _target;
    private float _currentFollowTime;

    public override void Start(Whiteboard data)
    {
        _agent = data.Get<UnityEngine.AI.NavMeshAgent>(Agent);
        _agent.SetDestination(_stateMachine.transform.position);
        _target = data.Get<GameObject>(Target);
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

    public override void Exit(Whiteboard data) { }
}
