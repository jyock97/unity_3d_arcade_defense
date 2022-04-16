using System;
using System.Collections;
using UnityEngine;

public class AreaProjectileController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float attackRadius;
    [SerializeField] private ParticleSystem _particleSystemProjectile;
    [SerializeField] private ParticleSystem _particleSystemTrail;
    [SerializeField] private ParticleSystem _particleSystemExplosion;
    
    private Vector3 _targetPosition;
    private AnimationCurve xCurve;
    private AnimationCurve zCurve;
    private AnimationCurve yCurve;
    
    private float _distanceToTarget;
    private float _timeToTarget;
    private float _currentTime;
    private bool _explode;

    private void Update()
    {
        if (_currentTime < _timeToTarget)
        {
            _currentTime += Time.deltaTime;
        } else if (!_explode)
        {
            _currentTime = _timeToTarget;
            Explode();
        }
        
        transform.position = new Vector3(xCurve.Evaluate(_currentTime), yCurve.Evaluate(_currentTime), zCurve.Evaluate(_currentTime));
    }

    private void Explode()
    {
        _explode = true;
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, TagsLayers.Player_NexoLayerMask);
        foreach (Collider hit in hits)
        {
            hit.GetComponent<EntityController>().DealDamage();
        }

        _particleSystemProjectile.gameObject.SetActive(false);
        _particleSystemTrail.Stop();
        _particleSystemExplosion.Play();
        StartCoroutine(Destroy());
    }
    
    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _distanceToTarget = Vector3.Distance(transform.position, _targetPosition);
        _timeToTarget = _distanceToTarget / speed;
        
        xCurve = AnimationCurve.Linear(0, transform.position.x, _timeToTarget, _targetPosition.x);
        zCurve = AnimationCurve.Linear(0, transform.position.z, _timeToTarget, _targetPosition.z);
        yCurve = AnimationCurve.Linear(0, 0, 0, 0);
        yCurve.keys = new Keyframe[]
        {
            new Keyframe(0, transform.position.y, 0, Mathf.PI),
            new Keyframe(_timeToTarget/2, transform.position.y + 2),
            new Keyframe(_timeToTarget, _targetPosition.y, -2 * Mathf.PI, 0)
        };
        
        _particleSystemProjectile.Play();
        _particleSystemTrail.Play();
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (GlobalGizmosController.Enemies &&
            GlobalGizmosController.AreaProjectile &&
            _explode)
        {
            Color color = Color.red;
            color.a = 0.5f;
            Gizmos.color = color;

            Gizmos.DrawSphere(transform.position, attackRadius);
        }
    }
}
