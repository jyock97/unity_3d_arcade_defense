using UnityEngine;

public class Enemy03StateMachine: EnemyStateMachine
{
    [Header("Enemy03AttackState")]
    [SerializeField] private float rotationFollowTime;
    [SerializeField] private GameObject areaDamage;
    
    private Enemy03AttackState S_Enemy03AttackState;
    
    private GameObject _attackTarget;

    protected override void Start()
    {
        base.Start();

        S_Enemy03AttackState = new Enemy03AttackState("Enemy03AttackState", this);
    }
    
    protected override void InitWhiteBoard()
    {
        base.InitWhiteBoard();
        
        _data.Set(Enemy03AttackState.Agent, _agent);
        _data.Set(Enemy03AttackState.Animator, _animator);
        _data.Set(Enemy03AttackState.FollowTime, rotationFollowTime);
    }

    protected override void AttackTarget(GameObject target, bool keepInitialPosition)
    {
        Debug.Log("Attack");
        
        _data.Set(Enemy03AttackState.Target, target);
        ChangeState(S_Enemy03AttackState);
        _attackTarget = target;
    }
    
    // Used By Animation
    private void Shoot()
    {
        GameObject go = Instantiate(areaDamage);
        go.transform.position = _attackTarget.transform.position;
        go.GetComponent<AreaDamageController>().SetTarget(_attackTarget);
    }
}