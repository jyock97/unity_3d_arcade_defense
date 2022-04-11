using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private int life;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float invulnerabilityTime;

    [HideInInspector]public Animator animator;
    [HideInInspector] public bool isDead;

    private Quaternion _lookRotation;
    private float _currentInvulnerabilityTime;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, rotationSpeed * Time.deltaTime);
    }

    private Quaternion LookYRotation(Vector3 target)
    {
        // Normalizing direction to avoid ignoring rotation when target 
        // position is to close to the current position.
        Quaternion q = Quaternion.LookRotation((target - transform.position).normalized);
        
        // Rotate only Y axis
        return Quaternion.Euler(new Vector3(0, q.eulerAngles.y, 0));
    }
    
    public void LookYRotationInstant(Vector3 target)
    {
        _lookRotation = LookYRotation(target);
        transform.rotation = LookYRotation(target);
    }
    
    public void LookYRotationOverTime(Vector3 target)
    {
        _lookRotation = LookYRotation(target);
    }

    public void DealDamage()
    {
        if (_currentInvulnerabilityTime < Time.time)
        {
            _currentInvulnerabilityTime = Time.time + invulnerabilityTime;
            life--;
            if (life > 0)
            {
                if (animator != null)
                {
                    animator.SetTrigger("hurt");
                }
            }
            else
            {
                Die();
            }
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
    }
}