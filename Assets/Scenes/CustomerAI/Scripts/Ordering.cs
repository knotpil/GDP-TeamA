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
        // Only take orders when player is in the ORDERING checkpoint
        if (Input.GetKeyDown(KeyCode.E) && currentCustomer != null && player.isInOrderingCheckpoint)
        {
            CustomerOrder customerScript = currentCustomer.GetComponent<CustomerOrder>();
            CustomerController controller = currentCustomer.GetComponent<CustomerController>();
            
            // Only take order if customer hasn't placed order yet AND isn't already going to waiting
            if (customerScript != null && !customerScript.placed && controller != null && !controller.shouldGoToWaiting)
            {
                Debug.Log("Success! Taking order from " + currentCustomer.name);
                
                orderQueue.Enqueue(customerScript.o);
                customerScript.placed = true;

                Renderer[] customerMat = currentCustomer.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in customerMat)
                {
                    r.material.color = Color.green;
                }

                // Send customer to waiting area after order is taken
                Debug.Log("Ordering: Sending customer to waiting area");
                controller.shouldGoToWaiting = true;
                
                // Only log order details if queue has items
                if (orderQueue.Count > 0)
                {
                    Debug.Log(orderQueue.Peek().r_ + " " + orderQueue.Peek().g_ + " " + orderQueue.Peek().b_);
                    Debug.Log(orderQueue.Peek().w_);
                    Debug.Log(orderQueue.Peek().hl_);
                    Debug.Log(orderQueue.Peek().s_);
                    Debug.Log(orderQueue.Peek().pattern_);
                    Debug.Log(orderQueue.Peek().p_);
                    Debug.Log("Order Cost: $" + orderQueue.Peek().cost_);
                    Debug.Log("Order #" + orderQueue.Peek().num_);
                    orderQueue.Dequeue(); //dequeued for proper order display (at least for debugging)
                }
            }
            else if (customerScript != null && customerScript.placed)
            {
                Debug.Log("Ordering: Customer has already placed order");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when a customer enters the ordering checkpoint
        if (other.CompareTag("Customer"))
        {
            CustomerOrder orderScript = other.GetComponent<CustomerOrder>();
            CustomerController controller = other.GetComponent<CustomerController>();
            
            // Only set as current customer if they haven't placed order and aren't going to waiting
            if (orderScript != null && !orderScript.placed && controller != null && !controller.shouldGoToWaiting)
            {
                currentCustomer = other.gameObject;
                Debug.Log("Ordering: Customer " + other.gameObject.name + " entered checkpoint (ready to order)");
            }
            else
            {
                Debug.Log("Ordering: Customer " + other.gameObject.name + " entered but not ready to order");
            }
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


