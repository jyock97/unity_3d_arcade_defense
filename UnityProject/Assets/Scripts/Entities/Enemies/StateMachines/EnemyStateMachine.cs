using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyStateMachine : StateMachine
{
    [SerializeField] private float sightRadius;
    [SerializeField] private float attackRadius;
    
    [Header("MoveToTargetState")]
    [SerializeField] private float speed;
    [SerializeField] private float distanceOffset;

    protected MoveToTargetState S_MoveToTargetState;
    
    protected GameObject _player;
    protected GameObject _nexo;
    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected Collider[] _colliders;
    protected bool _isAttacking;

    private bool _isChasingNexo;
    private bool _isChasingPlayer;
    private bool _defaultState;
    private bool _targetOnSight;

    protected override void Start()
    {
        S_MoveToTargetState = new MoveToTargetState(this);
        _player = GameObject.FindWithTag(TagsLayers.PlayerTag);
        _nexo = GameObject.FindWithTag(TagsLayers.NexoTag);
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _colliders = new Collider[2];
        
        _currentState = S_MoveToTargetState;
        
        base.Start();
    }

    protected override void Update()
    {
        UpdateData();

        base.Update();

        if (!_isAttacking)
        {
            int collisionCount = 0;
            _targetOnSight = false;
        
            // Detects the Nexo
            collisionCount = Physics.OverlapSphereNonAlloc(transform.position, sightRadius, _colliders, TagsLayers.NexoLayerMask);
            if (collisionCount > 0)
            {
                _targetOnSight = true;

                if (!_isChasingNexo)
                {
                    _defaultState = false;
                    _isChasingNexo = true;
                    RunToTarget(_nexo);
                }

            }
            else
            {
                // Detects the Player
                collisionCount = Physics.OverlapSphereNonAlloc(transform.position, sightRadius, _colliders, TagsLayers.PlayerLayerMask);
                if (collisionCount > 0)
                {
                    _targetOnSight = true;
                    if (!_isChasingPlayer)
                    {
                        _defaultState = false;
                        _isChasingPlayer = true;
                        RunToTarget(_player);
                    }
                }
            }

            if (_targetOnSight)
            {
                // Detects Nexo in attackRadius
                collisionCount = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, _colliders, TagsLayers.NexoLayerMask);
                if (collisionCount > 0)
                {
                    _defaultState = false;
                    _isChasingNexo = false;
                    _isChasingPlayer = false;
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
                        _isChasingNexo = false;
                        _isChasingPlayer = false;
                        _isAttacking = true;
                        AttackTarget(_player, false);
                    }
                }
            }
            else if (!_defaultState)
            {
                // Default state
                _defaultState = true;
                _isChasingNexo = false;
                _isChasingPlayer = false;
                _animator.SetBool("isMoving", false);
                _animator.SetBool("isRunning", false);
                _data.Set(MoveToTargetState.AnimatorMovingFlag, "isMoving");
                _data.Set(MoveToTargetState.Target, _nexo);
                _data.Set(MoveToTargetState.Speed, speed);
                ChangeState(S_MoveToTargetState);
            }
        }
    }

    protected virtual void UpdateData()
    {
        _data.Set(MoveToTargetState.DistanceOffset, distanceOffset);
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
    }

    private void RunToTarget(GameObject target)
    {
        _data.Set(MoveToTargetState.Target, target);
        _data.Set(MoveToTargetState.Speed, speed * 2);
        _data.Set(MoveToTargetState.FollowTarget, true);
        _data.Set(MoveToTargetState.AnimatorMovingFlag, "isRunning");
        ChangeState(S_MoveToTargetState);
    }

    protected abstract void AttackTarget(GameObject target, bool keepInitialPosition);

    // Used By Animation
    private void AttackEnd()
    {
        _isAttacking = false;
    }
    
    protected virtual void OnDrawGizmos()
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
        }
    }
}