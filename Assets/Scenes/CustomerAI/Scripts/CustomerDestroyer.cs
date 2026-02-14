using UnityEngine;

public class CustomerDestroyer : MonoBehaviour
{
	[Header("Auto-Regeneration (Optional)")]
	[Tooltip("If enabled, will automatically create a new customer when one is destroyed. Leave OFF if using CustomerSpawner.")]
	public bool autoRegenerate = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            Debug.Log("CustomerDestroyer: Customer reached exit, destroying...");
            CustomerController oldScript = other.gameObject.GetComponent<CustomerController>();
            CustomerOrder oldOrderScript = other.gameObject.GetComponent<CustomerOrder>();
			
			// Only regenerate if enabled and prefab exists
			if(autoRegenerate && oldScript.customerPrefab != null)
			{
				GameObject newCustomer = Instantiate(oldScript.customerPrefab);
				if(newCustomer != null)
				{
					CustomerController newScript = newCustomer.GetComponent<CustomerController>();
					CustomerOrder newOrderScript = newCustomer.GetComponent<CustomerOrder>();
					if(newScript != null)
					{
						newScript.counterTarget = oldScript.counterTarget;
						newScript.waitingArea = oldScript.waitingArea;
						newScript.exitPoint1 = oldScript.exitPoint1;
						newScript.exitPoint2 = oldScript.exitPoint2;
						newScript.customerPrefab = oldScript.customerPrefab;
						newScript.queueManager = oldScript.queueManager; // Pass queue manager reference
						newOrderScript.o.num_ = oldOrderScript.o.num_ + 1;
					} 
					else
					{
						Debug.LogWarning("CustomerDestroyer: New customer script missing on " + newCustomer.name);
					}
				} 
				else
				{
					Debug.LogWarning("CustomerDestroyer: New customer instantiation failed on " + other.gameObject.name);
				}
			}
			
			// Always destroy the old customer
            Destroy(other.gameObject);
        }
    }
}
