using System.Collections.Generic;
using UnityEngine;
using OrderOwner;

public class Ordering : MonoBehaviour
{
    [Header("Necessary Components")]
    public PlayerTrigger player;

    [Header("Current Customer")]
    public GameObject currentCustomer;

    [Header("Current Orders")]
    public Queue<Order> orderQueue = new Queue<Order>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentCustomer != null && player.checkpointTrigger)
        {
            Debug.Log("Success! Taking order from " + currentCustomer.name);
            
            CustomerOrder customerScript = currentCustomer.GetComponent<CustomerOrder>();
            if (customerScript != null && !customerScript.placed)
            {
                orderQueue.Enqueue(customerScript.o);
                customerScript.placed = true;
                
                // Send customer to waiting area after order is taken
                GoToWaiting waitingScript = currentCustomer.GetComponent<GoToWaiting>();
                if (waitingScript != null)
                {
                    Debug.Log("Ordering: Sending customer to waiting area");
                    waitingScript.GoToWaitingArea();
                }
                else
                {
                    Debug.LogError("Ordering: GoToWaiting script not found on " + currentCustomer.name);
                }
                
                // Only log order details if queue has items
                if (orderQueue.Count > 0)
                {
                    Debug.Log(orderQueue.Peek().r_ + orderQueue.Peek().g_ + orderQueue.Peek().b_);
                    Debug.Log(orderQueue.Peek().w_);
                    Debug.Log(orderQueue.Peek().hl_);
                    Debug.Log(orderQueue.Peek().s_);
                    Debug.Log(orderQueue.Peek().pattern_);
                    Debug.Log(orderQueue.Peek().p_);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when a customer enters the ordering checkpoint
        if (other.CompareTag("Customer"))
        {
            currentCustomer = other.gameObject;
            Debug.Log("Ordering: Customer " + other.gameObject.name + " entered checkpoint");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear reference when customer leaves
        if (other.CompareTag("Customer") && other.gameObject == currentCustomer)
        {
            currentCustomer = null;
            Debug.Log("Ordering: Customer left checkpoint");
        }
    }
}


