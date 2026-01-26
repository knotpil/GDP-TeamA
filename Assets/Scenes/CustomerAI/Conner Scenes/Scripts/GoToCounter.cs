using UnityEngine;
using UnityEngine.AI;

public class GoToCounter : MonoBehaviour
{
    [Header("Destination")]
    public Transform target;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        SetDestination();
    }

    void Update()
    {
        // Keeps following if the target moves
        if (target != null && agent.enabled)
        {
            agent.SetDestination(target.position);
        }
    }

    public void SetDestination()
    {
        if (target == null)
        {
            Debug.LogWarning("GoToTarget: No target assigned.", this);
            return;
        }

        if (!agent.enabled)
        {
            Debug.LogWarning("GoToTarget: NavMeshAgent is disabled.", this);
            return;
        }

        agent.SetDestination(target.position);
    }
}
