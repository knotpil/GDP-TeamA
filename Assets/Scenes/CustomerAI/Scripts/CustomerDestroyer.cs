using UnityEngine;

public class CustomerDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            Debug.Log("CustomerDestroyer: Customer reached exit, destroying...");
            CustomerController oldScript = other.gameObject.GetComponent<CustomerController>();
            CustomerOrder oldOrderScript = other.gameObject.GetComponent<CustomerOrder>();
			
			// Always destroy the old customer
            Destroy(other.gameObject);
        }
    }
}
