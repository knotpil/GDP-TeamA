using UnityEngine;
using UnityEngine.AI;

public class GoToWaiting : MonoBehaviour
{
    [Header("Waiting Area")]
    public Transform waitingArea; // Assign the waiting room cube in the Inspector

    [Header("Movement Settings")]
    public float moveSpeed = 3.5f;
    
    private NavMeshAgent agent;
    private bool shouldGoToWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            Debug.Log("GoToWaiting: NavMeshAgent found and configured on " + gameObject.name);
        }
        else
        {
            Debug.LogWarning("GoToWaiting: No NavMeshAgent found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (shouldGoToWaiting && waitingArea != null)
        {
            if (agent != null && agent.enabled)
            {
                if (!agent.hasPath || agent.remainingDistance < 0.5f)
                {
                    agent.SetDestination(waitingArea.position);
                }
                
                // Debug info
                if (agent.hasPath)
                {
                    Debug.Log("GoToWaiting: " + gameObject.name + " moving to waiting area. Distance: " + agent.remainingDistance);
                }
            }
            else if (agent == null)
            {
                // Fallback: Simple movement if no NavMeshAgent
                transform.position = Vector3.MoveTowards(transform.position, waitingArea.position, moveSpeed * Time.deltaTime);
                
                float distance = Vector3.Distance(transform.position, waitingArea.position);
                Debug.Log("GoToWaiting: " + gameObject.name + " moving (simple). Distance: " + distance);
            }
        }
    }

    public void GoToWaitingArea()
    {
        Debug.Log("GoToWaitingArea() called on " + gameObject.name);
        
        if (waitingArea == null)
        {
            Debug.LogError("GoToWaiting: Waiting area is not assigned in Inspector!");
            return;
        }
        
        // Disable GoToCounter script so it doesn't override our destination
        GoToCounter counterScript = GetComponent<GoToCounter>();
        if (counterScript != null)
        {
            counterScript.enabled = false;
            Debug.Log("GoToWaiting: Disabled GoToCounter script");
        }
        
        shouldGoToWaiting = true;
        
        if (agent != null)
        {
            if (!agent.enabled)
            {
                agent.enabled = true;
                Debug.Log("GoToWaiting: NavMeshAgent was disabled, enabling it now");
            }
            
            if (!agent.isOnNavMesh)
            {
                Debug.LogError("GoToWaiting: Agent is not on NavMesh! Make sure you have baked a NavMesh.");
            }
            else
            {
                agent.SetDestination(waitingArea.position);
                Debug.Log("GoToWaiting: Destination set to " + waitingArea.position);
            }
        }
        else
        {
            Debug.Log("GoToWaiting: Using simple movement (no NavMeshAgent)");
        }
    }
}
