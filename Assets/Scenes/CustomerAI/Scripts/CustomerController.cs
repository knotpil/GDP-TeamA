using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerController : MonoBehaviour
{
	private NavMeshAgent agent;

	[Header("Counter")]
	public Transform counterTarget;
	private bool counterReached = false;
	
	[Header("Waiting")]
	public Transform waitingArea;
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
		SetDestination();
		actualExit = UnityEngine.Random.Range(0, 2) == 0 ? exitPoint1 : exitPoint2;
		if(customerPrefab == null)
		{
			Debug.LogWarning("CustomerController: Prefab missing, regeneration unavailable on " + gameObject.name);
		}
	}

	void Update() 
	{
		if (agent.enabled)
		{
			SetDestination();
			if(!counterReached && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) 
			{
				counterReached = true;
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

		if (waitingArea == null)
		{
			Debug.LogWarning("CustomerController: No waiting area assigned.", this);
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

		if(!counterReached)
		{
			agent.SetDestination(counterTarget.position);
		}

	 	if (shouldGoToWaiting && !shouldLeave)
		{
			agent.SetDestination(waitingArea.position);
		}

		if (shouldLeave)
		{
			agent.SetDestination(actualExit.position);

			if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Debug.Log("CustomerController: Customer reached exit, destroying...");
				GameObject newCustomer = Instantiate(customerPrefab);
				if(newCustomer != null)
				{
					CustomerController newScript = newCustomer.GetComponent<CustomerController>();
					if(newScript != null)
					{
						newScript.counterTarget = counterTarget;
						newScript.waitingArea = waitingArea;
						newScript.exitPoint1 = exitPoint1;
						newScript.exitPoint2 = exitPoint2;
						newScript.customerPrefab = customerPrefab;
					} else
					{
						Debug.LogWarning("CustomerController: New customer script missing on " + newCustomer.name);
					}
				} else
				{
					Debug.LogWarning("CustomerController: New customer instantiation failed on " + gameObject.name);
				}
                Destroy(gameObject);
            }
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
		}

		if(secondsWaiting == 20)
		{
            foreach (Renderer r in customerMat)
            {
                r.material.color = Color.red;
            }

			smokeParticles.SetActive(true);
		}


		timeToggle = true;
	}
}
