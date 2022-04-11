using UnityEngine;
using UnityEngine.AI;

public class StructureSlowArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsLayers.EnemyTag))
        {
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            agent.speed /= 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagsLayers.EnemyTag))
        {
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            agent.speed *= 2;
        }
    }
}
