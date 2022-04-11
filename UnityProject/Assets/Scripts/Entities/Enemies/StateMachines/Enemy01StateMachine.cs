using UnityEngine;
using UnityEngine.AI;

public class Enemy01StateMachine : StateMachine
{
    [SerializeField] private float waitTime;
    [SerializeField] private float sightRadius;
    [SerializeField] private float attackRadius;
    
    [Header("MoveToTargetState")]
    [SerializeField] private float speed;
    [SerializeField] private float distanceOffset;

    [Header("Enemy01AttackState")]
    [SerializeField] private float rotationFollowTime;
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private Vector3 attackSize;
    
    
    private GameObject _player;
    private GameObject _nexo;
    private NavMeshAgent _agent;
    private Animator _animator;

    private Collider[] _colliders;
    private bool _isTargetLock;
    private bool _defaultState;
    private bool _targetOnSight;
    private bool _isAttacking;
    private Vector3 _attackPosition;
    
    
    protected override void OnValidate()
    {
        base.OnValidate();

        _attackPosition = transform.position;
    }

    protected override void Start()
    {
        _player = GameObject.FindWithTag(TagsLayers.PlayerTag);
        _nexo = GameObject.FindWithTag(TagsLayers.NexoTag);
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _colliders = new Collider[2];
        
        _currentState = MoveToTargetState.Instance(this);
        
        base.Start();
    }

    protected override void Update()
    {
        _data.Set(MoveToTargetState.DistanceOffset, distanceOffset);
        _data.Set(Enemy01Attack.FollowTime, rotationFollowTime);

        base.Update();

        if (!_isAttacking)
        {
            int collisionCount = 0;
            _targetOnSight = false;
        
            // Detects the Nexo
            collisionCount = Physics.OverlapSphereNonAlloc(transform.position, sightRadius, _colliders, TagsLayers.NexoLayerMask);
            if (collisionCount > 0)
            {
                _defaultState = false;
                _targetOnSight = true;
                RunToTarget(_nexo);
            }
            else
            {
                // Detects the Player
                collisionCount = Physics.OverlapSphereNonAlloc(transform.position, sightRadius, _colliders, TagsLayers.PlayerLayerMask);

                if (collisionCount > 0)
                {
                    _defaultState = false;
                    _targetOnSight = true;
                    RunToTarget(_player);
                }
            }
        
            if (_targetOnSight)
            {
                // Detects Nexo in attackRadius
                collisionCount = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, _colliders, TagsLayers.NexoLayerMask);
                if (collisionCount > 0)
                {
                    _defaultState = false;
                    _isAttacking = true;
                    AttackTarget(_nexo, true);
                }
                else
                {
                    // Detects Player in attackRadius
                    collisionCount = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, _colliders, TagsLayers.PlayerLayerMask);
                    if (collisionCount > 0)
                    {
                        _defaultState = false;
                        _isAttacking = true;
                        AttackTarget(_player, false);
                    }
                }
            }
            else if (!_defaultState)
            {
                // Default state
                _defaultState = true;
                _data.Set(MoveToTargetState.AnimatorMovingFlag, "isMoving");
                _data.Set(MoveToTargetState.Target, _nexo);
                _data.Set(MoveToTargetState.Speed, speed);
                ChangeState(MoveToTargetState.Instance(this));
            }
        }

        if (!_isAttacking)
        {
            _attackPosition = transform.position;
        }
    }

    protected override void InitWhiteBoard()
    {
        _data.Set(MoveToTargetState.Agent, _agent);
        _data.Set(MoveToTargetState.Animator, _animator); // TODO design some kind of global to be use here, Global whiteboard variables maybe
        _data.Set(MoveToTargetState.AnimatorMovingFlag, "isMoving");
        _data.Set(MoveToTargetState.Target, _nexo);
        _data.Set(MoveToTargetState.Speed, speed);
        _data.Set(MoveToTargetState.FollowTarget, false);
        _data.Set(MoveToTargetState.DistanceOffset, distanceOffset);
        
        _data.Set(Enemy01Attack.Agent, _agent);
        _data.Set(Enemy01Attack.Animator, _animator);
        _data.Set(Enemy01Attack.FollowTime, rotationFollowTime);
    }

    private void RunToTarget(GameObject target)
    {
        _data.Set(MoveToTargetState.Target, target);
        _data.Set(MoveToTargetState.Speed, speed * 2);
        _data.Set(MoveToTargetState.FollowTarget, true);
        _data.Set(MoveToTargetState.AnimatorMovingFlag, "isRunning");
        ChangeState(MoveToTargetState.Instance(this));
    }

    private void AttackTarget(GameObject target, bool keepInitialPosition)
    {
        _data.Set(Enemy01Attack.Target, target);
        _data.Set(Enemy01Attack.KeepInitialPosition, keepInitialPosition);
        ChangeState(Enemy01Attack.Instance(this));
        _attackPosition = transform.position;
    }

    private Vector3 CalculateHitBoxPosition()
    {
        Vector3 newPosition = _attackPosition;
        newPosition.y += attackOffset.y;
        newPosition += transform.forward * attackOffset.z;
        return newPosition;
    }

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

    private void AttackEnd()
    {
        _isAttacking = false;
    }

    [Header("Gizmos")]
    private float _onHitTime;
    private void OnDrawGizmos()
    {
        if (GlobalGizmosController.Enemies)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;

            if (GlobalGizmosController.SightRadius)
            {
                Gizmos.DrawSphere(transform.position, sightRadius);
            }
            
            if (GlobalGizmosController.AttackRadius)
            {
                Gizmos.DrawSphere(transform.position, attackRadius);
            }

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