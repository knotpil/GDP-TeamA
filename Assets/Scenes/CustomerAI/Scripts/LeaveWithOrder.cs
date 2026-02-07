using System.Collections.Generic;
using UnityEngine;

public class LeaveWithOrder : MonoBehaviour
{
    [Header("Necessary Components")]
    public PlayerTrigger player;

    [Header("Exit Points")]
    public Transform exitPoint1; // Assign first exit point in Inspector
    public Transform exitPoint2; // Assign second exit point in Inspector

    private List<GameObject> customersInArea = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("=== LeaveWithOrder: E PRESSED ===");
            Debug.Log("LeaveWithOrder: Customers in area: " + customersInArea.Count);
            if (customersInArea.Count > 0)
            {
                Debug.Log("LeaveWithOrder: First customer: " + customersInArea[0].name);
            }
            Debug.Log("LeaveWithOrder: Player null? " + (player == null));
            if (player != null)
            {
                Debug.Log("LeaveWithOrder: Player checkpoint trigger: " + player.checkpointTrigger);
            }
            Debug.Log("LeaveWithOrder: Exit1 assigned? " + (exitPoint1 != null));
            Debug.Log("LeaveWithOrder: Exit2 assigned? " + (exitPoint2 != null));
        }
        
        if (Input.GetKeyDown(KeyCode.E) && customersInArea.Count > 0 && player.checkpointTrigger)
        {
            Debug.Log("LeaveWithOrder: Sending customer to exit!");
            
            if (exitPoint1 == null || exitPoint2 == null)
            {
                Debug.LogError("LeaveWithOrder: Exit points not assigned!");
                return;
            }

            // Send first customer in the area to exit
            GameObject customer = customersInArea[0];

            //flags that order was placed
            customer.GetComponent<CustomerOrder>().received = true;

            // Randomly choose exit point
            Transform selectedExit = Random.Range(0, 2) == 0 ? exitPoint1 : exitPoint2;
            Debug.Log("LeaveWithOrder: Customer " + customer.name + " going to " + selectedExit.name);

            // Tell customer to leave
            CustomerController exitScript = customer.GetComponent<CustomerController>();
            if (exitScript != null)
            {
                exitScript.shouldLeave = true;
            }
            else
            {
                Debug.LogError("LeaveWithOrder: CustomerController script not found on " + customer.name);
            }

            // Remove customer from list
            customersInArea.Remove(customer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when a customer enters the waiting checkpoint
        if (other.CompareTag("Customer") && !customersInArea.Contains(other.gameObject))
        {
            customersInArea.Add(other.gameObject);
            Debug.Log("LeaveWithOrder: Customer " + other.gameObject.name + " entered checkpoint");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove customer when they leave
        if (other.CompareTag("Customer") && customersInArea.Contains(other.gameObject))
        {
            customersInArea.Remove(other.gameObject);
            Debug.Log("LeaveWithOrder: Customer left checkpoint");
        }
    }
}
