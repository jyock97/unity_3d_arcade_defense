using UnityEngine;

public class Enemy02StateMachine : EnemyStateMachine
{
    [Header("Enemy02AttackState")]
    [SerializeField] private float rotationFollowTime;
    [SerializeField] private GameObject projectile;

    private Enemy02AttackState S_Enemy02AttackState;
        
    private GameObject _attackTarget;
    private Vector3 _attackTargetPosition;


    protected override void Start()
    {
        base.Start();
        S_Enemy02AttackState = new Enemy02AttackState(this);
    }

    protected override void InitWhiteBoard()
    {
        base.InitWhiteBoard();
        
        _data.Set(Enemy02AttackState.Agent, _agent);
        _data.Set(Enemy02AttackState.Animator, _animator);
        _data.Set(Enemy02AttackState.FollowTime, rotationFollowTime);
    }
    
    protected override void UpdateData()
    {
        base.UpdateData();
        
        _data.Set(Enemy02AttackState.FollowTime, rotationFollowTime);
    }

    protected override void AttackTarget(GameObject target, bool keepInitialPosition)
    {
        _data.Set(Enemy02AttackState.Target, target);
        _data.Set(Enemy02AttackState.KeepInitialPosition, keepInitialPosition);
        ChangeState(S_Enemy02AttackState);
        _attackTarget = target;
    }

    // Used By Animation
    private void Shoot()
    {
        GameObject go = Instantiate(projectile);
        go.transform.position = transform.position;
        _attackTargetPosition = _attackTarget.transform.position;
        go.GetComponent<AreaProjectileController>().SetTarget(_attackTargetPosition);
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        if (GlobalGizmosController.Enemies)
        {

            if (GlobalGizmosController.Enemy02AttackTarget)
            {
                Color color = Color.red;
                color.a = 0.5f;
                Gizmos.color = color;
                
                Gizmos.DrawSphere(_attackTargetPosition, 0.5f);
            }
        }
    }
}
