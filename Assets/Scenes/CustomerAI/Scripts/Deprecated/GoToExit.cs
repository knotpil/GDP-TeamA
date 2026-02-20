using UnityEngine;
using UnityEngine.AI;

public class GoToExit : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform exitTarget;
    private bool shouldLeave = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogWarning("GoToExit: No NavMeshAgent found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (shouldLeave && exitTarget != null && agent != null && agent.enabled)
        {
            // Keep moving to exit
            agent.SetDestination(exitTarget.position);

            // Destroy customer when they reach the exit
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Debug.Log("GoToExit: Customer reached exit, destroying...");
                Destroy(gameObject);
            }
        }
    }

    public void LeaveToExit(Transform exit)
    {
        Debug.Log("GoToExit: Customer leaving to " + exit.name);

        exitTarget = exit;

        // Disable GoToWaiting script so it doesn't interfere
        GoToWaiting waitingScript = GetComponent<GoToWaiting>();
        if (waitingScript != null)
        {
            waitingScript.enabled = false;
            Debug.Log("GoToExit: Disabled GoToWaiting script");
        }

        shouldLeave = true;

        if (agent != null && agent.enabled)
        {
            agent.SetDestination(exitTarget.position);
        }
    }
}
