using System.Collections;
using UnityEngine;

public class AreaDamageController : MonoBehaviour
{
    [SerializeField] private float followTime;
    [SerializeField] private float lifeTime;
    [SerializeField] private float damageStartDelayTime;
    [SerializeField] private float attackRadius;
    [SerializeField] private ParticleSystem _particleSystem;

    private GameObject _target;
    private float _currentFollowTime;
    private float _currentLifeTime;

    private void Start()
    {
        _currentFollowTime = Time.time + followTime;
        _currentLifeTime = Time.time + lifeTime;

        StartCoroutine(Attack());
        _particleSystem.Play();
    }
    
    private void Update()
    {
        if (_currentFollowTime > Time.time)
        {
            transform.position = _target.transform.position;
        }

        if (_currentLifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(damageStartDelayTime);
        while (Application.isPlaying)
        {
            _onHitTime = Time.time + 0.1f;
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, TagsLayers.Player_NexoLayerMask);
            foreach (Collider hit in hits)
            {
                hit.GetComponent<EntityController>().DealDamage();
                
            }
            yield return new WaitForSeconds(damageStartDelayTime);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    [Header("Gizmos")]
    private bool _attack;
    private float _onHitTime;
    private void OnDrawGizmos()
    {
        if (GlobalGizmosController.Enemies)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;

            if (GlobalGizmosController.AreaDamagePosition)
            {
                Gizmos.DrawSphere(transform.position, 1f);
            }

            if (GlobalGizmosController.AreaDamageOnHit && _onHitTime > Time.time)
            {
                Gizmos.DrawSphere(transform.position, 1f);
            }
        }
    }
}
