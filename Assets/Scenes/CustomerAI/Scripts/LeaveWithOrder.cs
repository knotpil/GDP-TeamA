using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this script on the WAITING SPOT (where customer waits).
/// This GameObject should have a trigger collider that detects customers.
/// </summary>
public class LeaveWithOrder : MonoBehaviour
{
    [Header("Necessary Components")]
    public PlayerTrigger player;
    private CurrencyManager playerMoney;

    [Header("Exit Points")]
    public Transform exitPoint1;
    public Transform exitPoint2;
    
    [Header("Player Checkpoint")]
    [Tooltip("The player checkpoint GameObject that corresponds to THIS waiting spot")]
    public GameObject playerCheckpointObject;
    
    private PlayerCheckpointTrigger playerCheckpoint;

    private GameObject currentCustomer;
    private CustomerOrder currentCustomerOrder; // Cache the CustomerOrder component
    private CustomerController currentCustomerController; // Cache the CustomerController component
    public bool isOccupied = false; // Tracks if a customer is currently at this spot

    private void Awake()
    {
        playerMoney = player.GetComponentInParent<CurrencyManager>();
        
        //Debug.Log($"LeaveWithOrder ({gameObject.name}): Awake called");
        //Debug.Log($"  - playerCheckpointObject assigned: {(playerCheckpointObject != null ? playerCheckpointObject.name : "NULL")}");
        
        // Get the PlayerCheckpointTrigger component from the assigned GameObject
        if (playerCheckpointObject != null)
        {
            playerCheckpoint = playerCheckpointObject.GetComponent<PlayerCheckpointTrigger>();
            if (playerCheckpoint == null)
            {
                Debug.LogError($"LeaveWithOrder on {gameObject.name}: Player checkpoint object doesn't have PlayerCheckpointTrigger script!");
            }
            //else
            //{
            //    Debug.Log($"LeaveWithOrder on {gameObject.name}: Successfully found PlayerCheckpointTrigger component");
            //}
        }
        else
        {
            Debug.LogWarning($"LeaveWithOrder on {gameObject.name}: No player checkpoint assigned!");
        }
    }

    private void Update()
    {
        // Check if player is at the corresponding player checkpoint
        bool playerAtCheckpoint = (playerCheckpoint != null && playerCheckpoint.playerIsHere);
        
        // Only give order when player is at the RIGHT checkpoint and there's a customer here
        if (Input.GetKeyDown(KeyCode.E) && currentCustomer != null && playerAtCheckpoint)
        {
            if (exitPoint1 == null || exitPoint2 == null)
            {
                Debug.LogError("LeaveWithOrder: Exit points not assigned!");
                return;
            }

            // Use cached components (already verified in OnTriggerEnter)
            //Debug.Log("LeaveWithOrder: Sending customer to exit!");

            //flags that order was received and pays player
            currentCustomerOrder.received = true;
            playerMoney.payment(currentCustomerOrder.o.cost_ + currentCustomerOrder.o.tip_);

            // Randomly choose exit point
            Transform selectedExit = Random.Range(0, 2) == 0 ? exitPoint1 : exitPoint2;
            //Debug.Log("LeaveWithOrder: Customer " + currentCustomer.name + " going to " + selectedExit.name);

            // Tell customer to leave
            currentCustomerController.shouldLeave = true;

            // Clear customer and cache
            currentCustomer = null;
            currentCustomerOrder = null;
            currentCustomerController = null;
            isOccupied = false; // Mark spot as available
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when a customer enters THE CORRESPONDING waiting spot
        if (other.CompareTag("Customer") && currentCustomer == null)
        {
            CustomerOrder orderScript = other.GetComponent<CustomerOrder>();
            CustomerController controller = other.GetComponent<CustomerController>();
            
            // Only add if they've placed order, are going to waiting, AND this is their assigned spot
            if (orderScript != null && orderScript.placed && controller != null && controller.shouldGoToWaiting)
            {
                // Check if THIS waiting spot is the one assigned to the customer
                if (controller.assignedWaitingSpot == transform)
                {
                    // Cache customer and components
                    currentCustomer = other.gameObject;
                    currentCustomerOrder = orderScript;
                    currentCustomerController = controller;
                    isOccupied = true; // Mark spot as occupied
                    //Debug.Log($"LeaveWithOrder ({gameObject.name}): Customer {other.gameObject.name} arrived at THEIR assigned waiting spot");
                }
                //else
                //{
                //    Debug.Log($"LeaveWithOrder ({gameObject.name}): Customer {other.gameObject.name} passed through but not assigned here (assigned to {controller.assignedWaitingSpot?.name})");
                //}
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove customer when they leave
        if (other.CompareTag("Customer") && other.gameObject == currentCustomer)
        {
            currentCustomer = null;
            currentCustomerOrder = null;
            currentCustomerController = null;
            isOccupied = false; // Mark spot as available
            //Debug.Log("LeaveWithOrder: Customer left waiting spot");
        }
    }
}
