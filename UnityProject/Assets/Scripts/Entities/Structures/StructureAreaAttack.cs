using System;
using UnityEngine;
using UnityEngine.AI;

public class StructureAreaAttack : MonoBehaviour
{
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDelayTime;

    private float _currentAttackDelayTime;
    private void Update()
    {
        if (_currentAttackDelayTime < Time.time)
        {
            Attack();
        }
    }

    private void Attack()
    {
        _currentAttackDelayTime = Time.time + attackDelayTime;
        _onHitTime = Time.time + 0.25f;
        
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRadius, transform.up, 0, TagsLayers.EnemyLayerMask);
        
        foreach (RaycastHit raycastHit in hits)
        {
            raycastHit.collider.GetComponent<EnemyController>().DealDamage();
        }
    }

    [Header("Gizmos")]
    private float _onHitTime;
    private void OnDrawGizmos()
    {
        Color color = Color.blue;
        color.a = 0.5f;
        Gizmos.color = color;
        
        if (GlobalGizmosController.TowerAttackRange)
        {
            Gizmos.color = Color.blue;;
            
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
        if (GlobalGizmosController.TowerAttackHitBoxOnHit && Time.time < _onHitTime)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, attackRadius);
        }
    }
}
