using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Whiteboard usage
///  - Enemy01Attack_Agent -> expected NavMeshAgent
///  - Enemy01Attack_Animator -> expected Animator
///  - Enemy01Attack_Target -> expected GameObject
///  - Enemy01Attack_FollowTime -> expected float
///  - Enemy01Attack_KeepInitialPosition -> expected bool
/// </summary>
public class Enemy01Attack : State
{
    public static readonly string Agent = "Enemy01Attack_Agent";
    public static readonly string Animator = "Enemy01Attack_Animator";
    public static readonly string Target = "Enemy01Attack_Target";
    public static readonly string FollowTime = "Enemy01Attack_FollowTime";
    public static readonly string KeepInitialPosition = "Enemy01Attack_KeepInitialPosition";

    public Enemy01Attack(StateMachine stateMachine) : base("Enemy01Attack", stateMachine) { }
    private static State _instance;
    public static State Instance(StateMachine stateMachine)
    {
        return _instance ?? (_instance = new Enemy01Attack(stateMachine));
    }
    
    
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