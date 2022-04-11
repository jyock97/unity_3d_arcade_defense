using UnityEngine;

public class Enemy01StateMachine : EnemyStateMachine
{
    [Header("Enemy01AttackState")]
    [SerializeField] private float rotationFollowTime;
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private Vector3 attackSize;

    private Enemy01AttackState S_Enemy01AttackState;
        
    private Vector3 _attackPosition;
    private bool _attackKeepInitialPosition;

    protected override void OnValidate()
    {
        base.OnValidate();

        _attackPosition = transform.position;
    }

    protected override void Start()
    {
        S_Enemy01AttackState = new Enemy01AttackState(this);
        
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!_isAttacking || _attackKeepInitialPosition)
        {
            _attackPosition = transform.position;
        }
    }

    protected override void InitWhiteBoard()
    {
        base.InitWhiteBoard();
        
        _data.Set(Enemy01AttackState.Agent, _agent);
        _data.Set(Enemy01AttackState.Animator, _animator);
        _data.Set(Enemy01AttackState.FollowTime, rotationFollowTime);
    }

    protected override void UpdateData()
    {
        base.UpdateData();
        
        _data.Set(Enemy01AttackState.FollowTime, rotationFollowTime);
    }

    protected override void AttackTarget(GameObject target, bool keepInitialPosition)
    {
        _attackPosition = transform.position;
        _attackKeepInitialPosition = keepInitialPosition;
        _data.Set(Enemy01AttackState.Target, target);
        _data.Set(Enemy01AttackState.KeepInitialPosition, _attackKeepInitialPosition);
        ChangeState(S_Enemy01AttackState);
    }
    
    private Vector3 CalculateHitBoxPosition()
    {
        Vector3 newPosition = _attackPosition;
        newPosition.y += attackOffset.y;
        newPosition += transform.forward * attackOffset.z;
        return newPosition;
    }
    

    // Used By Animation
    private void MeleeAttackHit()
    {
        _onHitTime = Time.time + 0.25f;
        int collisionCount = 0;
        collisionCount = Physics.OverlapBoxNonAlloc(CalculateHitBoxPosition(), attackSize / 2, _colliders, transform.rotation, TagsLayers.Player_NexoLayerMask);
        
        for (int i = 0; i < collisionCount; i++)
        {
            
            _colliders[i].gameObject.GetComponent<EntityController>()?.DealDamage();
        }
    }

    [Header("Gizmos")]
    private float _onHitTime;
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        if (GlobalGizmosController.Enemies)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;

            if (GlobalGizmosController.Enemy01AttackHitBoxAlways)
            {
                GizmosUtils.DrawCube(CalculateHitBoxPosition(), attackSize, transform.rotation);
            }

            if (GlobalGizmosController.Enemy01AttackHitBoxOnHit && Time.time < _onHitTime)
            {
                GizmosUtils.DrawCube(CalculateHitBoxPosition(), attackSize, transform.rotation);
            }
        }
    }
}