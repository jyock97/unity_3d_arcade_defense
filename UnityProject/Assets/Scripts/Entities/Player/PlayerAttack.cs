using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject hitColliderRootObj;
    [SerializeField] private float hitColliderRadius;
    [SerializeField] private Vector3 hitColliderOffset;

    private PlayerController _playerController;
    private Animator _animator;
    private NavMeshAgent _agent;
    private float _currentComboThresholdTime;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!_playerController.isDead && !_playerController.isBuilding &&
            !_playerController.isDashing && Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        _playerController.isAttacking = true;
        
        _playerController.RaycastMouseToGround(out RaycastHit hit);
        if (hit.collider != null)
        {
            _playerController.LookYRotationInstant(hit.point);
        }

        _animator.SetTrigger("attack");
        _agent.enabled = false;
    }

    private Vector3 CalculateHitBoxPosition()
    {
        Vector3 newPosition = hitColliderRootObj.transform.position;
        newPosition.y += hitColliderOffset.y;
        newPosition += hitColliderRootObj.transform.forward * hitColliderOffset.z;
        return newPosition;
    }
    
    // Use by animation
    private void MeleeAttackHit()
    {
        _onHitTime = Time.time + 0.25f;
        
        RaycastHit[] hits = Physics.SphereCastAll(CalculateHitBoxPosition(), hitColliderRadius, transform.forward, 0, TagsLayers.EnemyLayerMask);
        
        foreach (RaycastHit raycastHit in hits)
        {
            raycastHit.collider.gameObject.GetComponent<EnemyController>().DealDamage();
        }
    }
    // Use by animation
    private void MeleeAttackEnd()
    {
        _playerController.isAttacking = false;
        _agent.enabled = true;
    }

    [Header("Gizmos")]
    private float _onHitTime;
    private void OnDrawGizmos()
    {
        if (GlobalGizmosController.Player)
        {
            Color color = Color.green;
            color.a = 0.5f;
            Gizmos.color = color;

            if (GlobalGizmosController.AttackHitBoxAlways)
            {
                Gizmos.DrawSphere(CalculateHitBoxPosition(), hitColliderRadius);
            }
            
            if (GlobalGizmosController.AttackHitBoxOnHit && Time.time < _onHitTime)
            {
                Gizmos.DrawSphere(CalculateHitBoxPosition(), hitColliderRadius);
            }
        }
    }
}
