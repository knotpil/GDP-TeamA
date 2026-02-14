using System.Collections.Generic;
using UnityEngine;

public class LeaveWithOrder : MonoBehaviour
{
    [Header("Necessary Components")]
    public PlayerTrigger player;
    private CurrencyManager playerMoney;

    [Header("Exit Points")]
    public Transform exitPoint1; // Assign first exit point in Inspector
    public Transform exitPoint2; // Assign second exit point in Inspector

    private List<GameObject> customersInArea = new List<GameObject>();
    private PlayerTriggerWaiting waitingTrigger;

    private void Awake()
    {
        playerMoney = player.GetComponentInParent<CurrencyManager>();
        // Get reference to this checkpoint's trigger component
        waitingTrigger = GetComponent<PlayerTriggerWaiting>();
        
        //Debug.Log($"LeaveWithOrder: Initialized on {gameObject.name}");
        //Debug.Log($"LeaveWithOrder: PlayerTriggerWaiting component found: {waitingTrigger != null}");
        //Debug.Log($"LeaveWithOrder: Player reference assigned: {player != null}");
    }

    private void Update()
    {
        // Check if player is in waiting checkpoint (supports both detection methods)
        bool playerInWaitingCheckpoint = (player != null && player.isInWaitingCheckpoint) || 
                                          (waitingTrigger != null && waitingTrigger.waitingCheckpointTrigger);
        
        // Debug: Check E key press
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Debug.Log($"LeaveWithOrder: E pressed - Customers in area: {customersInArea.Count}, Player in checkpoint: {playerInWaitingCheckpoint}");
        //}
        
        // Only give orders when player is in the WAITING checkpoint
        if (Input.GetKeyDown(KeyCode.E) && customersInArea.Count > 0 && playerInWaitingCheckpoint)
        {
            if (exitPoint1 == null || exitPoint2 == null)
            {
                Debug.LogError("LeaveWithOrder: Exit points not assigned!");
                return;
            }

            // Get first customer in the area
            GameObject customer = customersInArea[0];
            
            // Only send customer to exit if they've placed order and are in waiting area
            CustomerOrder orderScript = customer.GetComponent<CustomerOrder>();
            CustomerController controller = customer.GetComponent<CustomerController>();
            
            if (orderScript == null || !orderScript.placed)
            {
                Debug.Log("LeaveWithOrder: Customer hasn't placed order yet, ignoring");
                return;
            }
            
            if (controller != null && !controller.shouldGoToWaiting)
            {
                Debug.Log("LeaveWithOrder: Customer isn't in waiting area yet, ignoring");
                return;
            }
            
            Debug.Log("LeaveWithOrder: Sending customer to exit!");

            //flags that order was received and pays player
            orderScript.received = true;
            playerMoney.payment(orderScript.o.cost_ + orderScript.o.tip_);

            // Randomly choose exit point
            Transform selectedExit = Random.Range(0, 2) == 0 ? exitPoint1 : exitPoint2;
            Debug.Log("LeaveWithOrder: Customer " + customer.name + " going to " + selectedExit.name);

            // Tell customer to leave
            if (controller != null)
            {
                controller.shouldLeave = true;
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
        if (other.CompareTag("Customer"))
        {
            CustomerOrder orderScript = other.GetComponent<CustomerOrder>();
            CustomerController controller = other.GetComponent<CustomerController>();
            
            //Debug.Log($"LeaveWithOrder: Customer {other.gameObject.name} entering - Order placed: {orderScript?.placed}, Going to waiting: {controller?.shouldGoToWaiting}");
            
            // Only add to waiting area if they've placed order and are going to waiting
            if (orderScript != null && orderScript.placed && controller != null && controller.shouldGoToWaiting && !customersInArea.Contains(other.gameObject))
            {
                customersInArea.Add(other.gameObject);
                //Debug.Log("LeaveWithOrder: Customer " + other.gameObject.name + " ADDED to waiting area (Total: " + customersInArea.Count + ")");
            }
            //else if (!customersInArea.Contains(other.gameObject))
            //{
            //    Debug.Log("LeaveWithOrder: Customer " + other.gameObject.name + " entered but not in waiting state (not added)");
            //}
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
