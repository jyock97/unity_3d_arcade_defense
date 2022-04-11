using UnityEngine;
using UnityEngine.AI;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    
    private PlayerController _playerController;
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;
    private float _nextDashTime;
    private float _agentSpeed;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _agent = GetComponent<NavMeshAgent>();
        _agentSpeed = _agent.speed;
    }
    
    private void Update()
    {

        if (!_playerController.isDead &&
            _nextDashTime < Time.time && Input.GetKeyDown(KeyCode.Space))
        {
            _playerController.RaycastMouseToGround(out RaycastHit hit);
            if (hit.collider != null)
            {
                _playerController.isDashing = true;
                _nextDashTime = Time.time + dashCooldown;
                _dashStartPosition = transform.position;
                _targetPosition = transform.position + (hit.point - transform.position).normalized * dashDistance;
                _agent.enabled = false;

                _playerController.LookYRotationInstant(hit.point);
            }
        }

        if (_playerController.isDashing)
        {
            Dash();
        }

        if (Vector3.Distance(transform.position, _targetPosition) < 0.1)
        {
            StopDash();
        }
    }

    private void Dash()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, dashSpeed * Time.deltaTime);
    }

    private void StopDash()
    {
        _playerController.isDashing = false;
        _agent.enabled = true;
        _agent.SetDestination(_targetPosition);
    }

    [Header("Gizmos")]
    private Vector3 _dashStartPosition;
    private void OnDrawGizmos()
    {
        if (GlobalGizmosController.Player && GlobalGizmosController.DashDirection)
        {
            Color color = Color.green;
            color.a = 0.5f;
            Gizmos.color = color;
            
            Gizmos.DrawLine(_dashStartPosition, _targetPosition);
        }
    }
}
