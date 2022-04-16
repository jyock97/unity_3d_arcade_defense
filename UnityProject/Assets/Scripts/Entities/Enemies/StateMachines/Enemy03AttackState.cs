using UnityEngine;

/// <summary>
/// Whiteboard usage
///  - Enemy03AttackState_Agent -> expected NavMeshAgent
///  - Enemy03AttackState_Animator -> expected Animator
///  - Enemy03AttackState_Target -> expected GameObject
///  - Enemy03AttackState_FollowTime -> expected float
/// </summary>
public class Enemy03AttackState : State
{
    public static readonly string Agent = "Enemy03AttackState_Agent";
    public static readonly string Animator = "Enemy03AttackState_Animator";
    public static readonly string Target = "Enemy03AttackState_Target";
    public static readonly string FollowTime = "Enemy03AttackState_FollowTime";
    
    public Enemy03AttackState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

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