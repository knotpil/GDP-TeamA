using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerController : MonoBehaviour
{
	private NavMeshAgent agent;

	[Header("Queue System")]
	public CustomerQueueManager queueManager;
	private Transform currentQueuePosition;
	private bool inQueue = false;

	[Header("Counter")]
	public Transform counterTarget;
	private bool counterReached = false;
	
	[Header("Waiting")]
	public Transform[] waitingSpots; // Array of 3 waiting positions
	public Transform assignedWaitingSpot; // Which waiting spot this customer is assigned to
	public bool shouldGoToWaiting = false;

	[Header("Exiting")]
	public Transform exitPoint1;
	public Transform exitPoint2;
	private Transform actualExit;
	public bool shouldLeave = false;

	[Header("Customer Regeneration")]
	public GameObject customerPrefab;

	[Header("Emotions")]
	public GameObject smokeParticles;
	private CustomerOrder order;
	private bool timeToggle = true;
	private int secondsWaiting = 0;
	private Renderer[] customerMat;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		if(agent == null) 
		{
			Debug.LogWarning("CustomerController: No NavMeshAgent found on " + gameObject.name);
		}

		order = GetComponent<CustomerOrder>();
		if(order == null)
		{
			Debug.LogWarning("CustomerController: Order script not found on " + gameObject.name);
		}

		customerMat = GetComponentsInChildren<Renderer>();
		if(customerMat.Length < 5)
		{
			Debug.LogWarning("CustomerController: Missing material refs on " + gameObject.name);
		}

		if (smokeParticles == null)
		{
			Debug.LogWarning("CustomerController: Missing anger particles on " + gameObject.name);
		}
	}

	void Start()
	{
		actualExit = UnityEngine.Random.Range(0, 2) == 0 ? exitPoint1 : exitPoint2;
		if(customerPrefab == null)
		{
			Debug.LogWarning("CustomerController: Prefab missing, regeneration unavailable on " + gameObject.name);
		}

		// Register with queue manager
		if(queueManager != null)
		{
			bool added = queueManager.AddCustomerToQueue(this);
			if(added)
			{
				inQueue = true;
				Debug.Log($"CustomerController: {gameObject.name} successfully added to queue system");
			}
			else
			{
				Debug.LogWarning("CustomerController: Could not add to queue, using old behavior on " + gameObject.name);
				SetDestination();
			}
		}
		else
		{
			Debug.LogWarning($"CustomerController: {gameObject.name} has no queue manager assigned, using old counter behavior");
			// Fallback to old behavior if no queue manager
			SetDestination();
		}
	}

	void Update() 
	{
		if (agent.enabled)
		{
			SetDestination();
			
			// Only set counterReached for non-queue customers or when at front of queue
			if(!counterReached && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) 
			{
				// If using queue system, only mark as reached if at position 0
				if(inQueue && currentQueuePosition != null)
				{
					if(currentQueuePosition.name.Contains("QueuePosition_0"))
					{
						counterReached = true;
					}
				}
				else if(!inQueue)
				{
					// Old behavior - reaching counter target
					counterReached = true;
				}
			}

			if(order.placed && !order.received && timeToggle)
			{
				StartCoroutine(timerTick());
			} 
			else if (order.received && smokeParticles.activeSelf)
			{
				smokeParticles.SetActive(false);

				foreach (Renderer r in customerMat)
            	{	
                	r.material.color = Color.white;
            	}
			}
		}
	}

	public void SetDestination()
	{
		if (counterTarget == null)
		{
			Debug.LogWarning("CustomerController: No counter target assigned.", this);
			return;
		}

		if (waitingSpots == null || waitingSpots.Length == 0)
		{
			Debug.LogWarning("CustomerController: No waiting spots assigned.", this);
			return;
		}

		if (exitPoint1 == null || exitPoint2 == null)
		{
			Debug.LogWarning("CustomerController: Missing exit point.", this);
			return;
		}

		if(!agent.enabled)
		{
			Debug.LogWarning("CustomerController: NavMeshAgent is disabled", this);
			return;
		}

		// If in queue system, go to assigned queue position
		if(inQueue && currentQueuePosition != null && !counterReached && !shouldGoToWaiting)
		{
			agent.SetDestination(currentQueuePosition.position);
		}
		else if(!counterReached && !inQueue && !shouldGoToWaiting)
		{
			// Fallback to old counter target behavior
			agent.SetDestination(counterTarget.position);
		}

	 	if (shouldGoToWaiting && !shouldLeave)
		{
			// Remove from queue when going to waiting area so next customer can advance
			if(inQueue && queueManager != null)
			{
				queueManager.RemoveCustomerFromQueue(this);
				inQueue = false;
				//Debug.Log($"CustomerController: {gameObject.name} leaving queue for waiting area");
			}
			
			// Pick an available waiting spot if not assigned yet
			if(assignedWaitingSpot == null && waitingSpots.Length > 0)
			{
				// Find all available spots
				System.Collections.Generic.List<Transform> availableSpots = new System.Collections.Generic.List<Transform>();
				foreach(Transform spot in waitingSpots)
				{
					LeaveWithOrder leaveScript = spot.GetComponent<LeaveWithOrder>();
					if(leaveScript != null && !leaveScript.isOccupied)
					{
						availableSpots.Add(spot);
					}
				}
				
				// Pick a random available spot
				if(availableSpots.Count > 0)
				{
					int randomIndex = Random.Range(0, availableSpots.Count);
					assignedWaitingSpot = availableSpots[randomIndex];
					//Debug.Log($"CustomerController: {gameObject.name} assigned to {assignedWaitingSpot.name}");
				}
				else
				{
					Debug.LogWarning($"CustomerController: {gameObject.name} - All waiting spots occupied! Waiting...");
				}
			}
			
			// Go to assigned spot
			if(assignedWaitingSpot != null)
			{
				agent.SetDestination(assignedWaitingSpot.position);
			}
		}

		if (shouldLeave)
		{
			// Remove from queue when leaving
			if(inQueue && queueManager != null)
			{
				queueManager.RemoveCustomerFromQueue(this);
				inQueue = false;
			}
			agent.SetDestination(actualExit.position);
            //CustomerDestroyer will handle the rest now!
		}
	}

	public IEnumerator timerTick()
	{
		timeToggle = false;
		yield return new WaitForSeconds(1f);
		++secondsWaiting;

		if(secondsWaiting == 10)
		{
            foreach (Renderer r in customerMat)
            {
                r.material.color = Color.yellow;
            }
			order.o.tip_ /= 2;
		}

		if(secondsWaiting == 20)
		{
            foreach (Renderer r in customerMat)
            {
                r.material.color = Color.red;
            }
			
			order.o.tip_ = 0;
			smokeParticles.SetActive(true);
		}


		timeToggle = true;
	}

	/// <summary>
	/// Called by CustomerQueueManager to assign this customer a position in the queue
	/// </summary>
	public void AssignQueuePosition(Transform queuePosition)
	{
		currentQueuePosition = queuePosition;
		//Debug.Log($"CustomerController: {gameObject.name} assigned to queue position {queuePosition.name}");
		
		// If this is the front position (position 0), mark as reached counter
		if(queuePosition != null && queuePosition.name.Contains("QueuePosition_0"))
		{
			counterReached = true;
			//Debug.Log($"CustomerController: {gameObject.name} reached front of queue (counter)");
		}
		else
		{
			counterReached = false;
		}
	}
}
