using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
	private NavMeshAgent agent;

	[Header("Counter")]
	public Transform counterTarget;
	private bool counterReached = false;
	
	[Header(“Waiting”)]
	public Transform waitingArea;
	public bool shouldGoToWaiting = false;

	[Header(“Exiting”)]
	private Transform exitTarget;
	public bool shouldLeave = false;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		if(agent == null) 
		{
			Debug.LogWarning(“CustomerController: No NavMeshAgent found on “ + gameObject.name);
		}
	}

	void Start()
	{
		SetDestination();
	}

	void Update() 
	{
		if (agent.enabled)
		{
			SetDestination();
			if(!counterReached && !agent.pathPending && agent.remainingDistance <= agent.StoppingDistance) 
			{
				counterReached = true;
			}
		}
	}

	public void SetDestination()
	{
		if (counterTarget == null)
		{
			Debug.LogWarning(“CustomerController: No counter target assigned.”, this);
			return;
		}

		if (waitingArea == null)
		{
			Debug.LogWarning(“CustomerController: No waiting area assigned.”, this);
			return;
		}

		if (exitTarget == null)
		{
			Debug.LogWarning(“CustomerController: No exit target assigned.”, this);
			return;
		}

		if(!agent.enabled)
		{
			Debug.LogWarning(“CustomerController: NavMeshAgent is disabled”, this);
			return;
		}

		if(!counterReached)
		{
			agent.SetDestination(counterTarget.position);
		} 
		else if (shouldGoToWaiting)
		{
			agent.SetDestination(walkingArea.position);
		} 
		else if (shouldLeave)
		{
			agent.SetDestination(exitTarget.position);
		}
	}
}
