using UnityEngine;
using System.Collections.Generic;

public class CustomerQueueManager : MonoBehaviour
{
    [Header("Queue Configuration")]
    [Tooltip("Array of queue positions from front (position 0) to back")]
    public Transform[] queuePositions;
    
    [Header("Optional: Auto-Generate Queue Positions")]
    [Tooltip("If true, will auto-generate queue positions based on settings below")]
    public bool autoGeneratePositions = false;
    public int numberOfPositions = 5;
    public float spacingBetweenPositions = 2f;
    public Vector3 queueDirection = Vector3.back; // Direction from front to back of line
    
    // List to track customers in queue (index 0 is front of line)
    private List<CustomerController> customersInQueue = new List<CustomerController>();
    
    void Awake()
    {
        if (autoGeneratePositions)
        {
            GenerateQueuePositions();
        }
        
        if (queuePositions == null || queuePositions.Length == 0)
        {
            Debug.LogError("CustomerQueueManager: No queue positions assigned!");
        }
    }
    
    void GenerateQueuePositions()
    {
        // Create a parent object to hold all queue positions
        GameObject queueParent = new GameObject("QueuePositions");
        queueParent.transform.SetParent(transform);
        queueParent.transform.localPosition = Vector3.zero;
        
        queuePositions = new Transform[numberOfPositions];
        
        for (int i = 0; i < numberOfPositions; i++)
        {
            GameObject positionMarker = new GameObject($"QueuePosition_{i}");
            positionMarker.transform.SetParent(queueParent.transform);
            positionMarker.transform.position = transform.position + (queueDirection.normalized * spacingBetweenPositions * i);
            queuePositions[i] = positionMarker.transform;
        }
        
        Debug.Log($"CustomerQueueManager: Generated {numberOfPositions} queue positions");
    }
    
    /// <summary>
    /// Add a customer to the queue
    /// </summary>
    public bool AddCustomerToQueue(CustomerController customer)
    {
        if (customer == null)
        {
            Debug.LogWarning("CustomerQueueManager: Attempted to add null customer to queue");
            return false;
        }
        
        if (customersInQueue.Contains(customer))
        {
            Debug.LogWarning($"CustomerQueueManager: Customer {customer.gameObject.name} already in queue");
            return false;
        }
        
        if (customersInQueue.Count >= queuePositions.Length)
        {
            Debug.LogWarning("CustomerQueueManager: Queue is full!");
            return false;
        }
        
        customersInQueue.Add(customer);
        int position = customersInQueue.Count - 1;
        
        customer.AssignQueuePosition(queuePositions[position]);
        
        Debug.Log($"CustomerQueueManager: Added {customer.gameObject.name} to queue at position {position}");
        return true;
    }
    
    /// <summary>
    /// Remove a customer from the queue and advance everyone forward
    /// </summary>
    public void RemoveCustomerFromQueue(CustomerController customer)
    {
        if (customer == null || !customersInQueue.Contains(customer))
        {
            return;
        }
        
        int removedIndex = customersInQueue.IndexOf(customer);
        customersInQueue.Remove(customer);
        
        Debug.Log($"CustomerQueueManager: Removed {customer.gameObject.name} from position {removedIndex}");
        
        // Move everyone forward one position
        AdvanceQueue();
    }
    
    /// <summary>
    /// Advance all customers forward one position in the queue
    /// </summary>
    private void AdvanceQueue()
    {
        for (int i = 0; i < customersInQueue.Count; i++)
        {
            if (customersInQueue[i] != null && i < queuePositions.Length)
            {
                customersInQueue[i].AssignQueuePosition(queuePositions[i]);
                Debug.Log($"CustomerQueueManager: Advanced {customersInQueue[i].gameObject.name} to position {i}");
            }
        }
    }
    
    /// <summary>
    /// Get the customer at the front of the queue (position 0)
    /// </summary>
    public CustomerController GetFrontCustomer()
    {
        if (customersInQueue.Count > 0)
        {
            return customersInQueue[0];
        }
        return null;
    }
    
    /// <summary>
    /// Check if queue has space
    /// </summary>
    public bool HasSpace()
    {
        return customersInQueue.Count < queuePositions.Length;
    }
    
    /// <summary>
    /// Get the current number of customers in queue
    /// </summary>
    public int GetQueueCount()
    {
        return customersInQueue.Count;
    }
    
    // Visualize queue positions in editor
    void OnDrawGizmos()
    {
        if (queuePositions != null && queuePositions.Length > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < queuePositions.Length; i++)
            {
                if (queuePositions[i] != null)
                {
                    Gizmos.DrawWireSphere(queuePositions[i].position, 0.5f);
                    
                    // Draw line to next position
                    if (i < queuePositions.Length - 1 && queuePositions[i + 1] != null)
                    {
                        Gizmos.DrawLine(queuePositions[i].position, queuePositions[i + 1].position);
                    }
                }
            }
        }
    }
}
