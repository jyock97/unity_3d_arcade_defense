using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float mouseRaycastDistance;

    private PlayerController _playerController;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Vector3 _mousePosition;
    private Ray _mouseScreenToWorldRay;


    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false; // TODO validate if agent cant handle this by itself (now that we rotate to intermediate points)
    }
    
    private void Update()
    {
        if (!_playerController.isDead && !_playerController.isDashing && 
            !_playerController.isAttacking)
        {
            if (Input.GetMouseButton(1))
            {
                _currentGizmosDrawTime = Time.time + gizmosDrawTime;

                _playerController.RaycastMouseToGround(out RaycastHit hit);
                if (hit.collider != null)
                {
                    _raycastHitPoint = hit.point;
                    _agent.SetDestination(hit.point);
                }
            }

            if (_agent.enabled && _agent.hasPath)
            {
                _playerController.LookYRotationOverTime(_agent.path.corners[1]);
            }
        }

        _animator.SetBool("isMoving", !_playerController.isAttacking && _agent.enabled && _agent.remainingDistance > 0.1);
    }

    [Header("Gizmos")]
    [SerializeField] private float gizmosDrawTime;
    private float _currentGizmosDrawTime;
    private Vector3 _raycastHitPoint;
    private void OnDrawGizmos()
    {
        if (GlobalGizmosController.Player && GlobalGizmosController.LeftMouseClick)
        {
            Color color = Color.green;
            color.a = 0.5f;
            Gizmos.color = color;

            Gizmos.DrawSphere(_raycastHitPoint, 0.125f);
            if (_currentGizmosDrawTime > Time.time)
            {
                Gizmos.DrawRay(_mouseScreenToWorldRay.origin, _mouseScreenToWorldRay.direction * mouseRaycastDistance);
            }
        }
    }
}
