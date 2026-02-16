using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [Tooltip("The customer prefab to spawn")]
    public GameObject customerPrefab;
    
    [Tooltip("Where customers will spawn")]
    public Transform spawnPoint;
    
    [Tooltip("Maximum number of customers that can exist at once")]
    public int maxActiveCustomers = 5;
    
    [Tooltip("Time in seconds between spawn attempts")]
    public float spawnInterval = 3f;
    
    [Tooltip("Start spawning customers automatically when game starts")]
    public bool autoSpawn = true;
    
    [Header("References")]
    [Tooltip("Queue manager to assign to spawned customers")]
    public CustomerQueueManager queueManager;
    
    [Tooltip("Counter target to assign to spawned customers")]
    public Transform counterTarget;
    
    [Tooltip("Array of 3 waiting spots where customers can wait")]
    public Transform[] waitingSpots;
    
    [Tooltip("Exit points to assign to spawned customers")]
    public Transform exitPoint1;
    public Transform exitPoint2;
    
    [Header("Runtime Info")]
    [Tooltip("Current number of active customers (read-only)")]
    [SerializeField] private int currentCustomerCount = 0;
    
    private List<GameObject> activeCustomers = new List<GameObject>();
    private bool isSpawning = false;
    private int customerNumber = 0;
    
    void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
            Debug.LogWarning("CustomerSpawner: No spawn point assigned, using spawner position");
        }
        
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerSpawner: No customer prefab assigned!");
            return;
        }
        
        if (autoSpawn)
        {
            StartSpawning();
        }
    }
    
    void Update()
    {
        // Clean up null references (destroyed customers)
        activeCustomers.RemoveAll(c => c == null);
        currentCustomerCount = activeCustomers.Count;
    }
    
    /// <summary>
    /// Start spawning customers at the set interval
    /// </summary>
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
            Debug.Log("CustomerSpawner: Started spawning customers");
        }
    }
    
    /// <summary>
    /// Stop spawning new customers
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log("CustomerSpawner: Stopped spawning customers");
    }
    
    /// <summary>
    /// Manually spawn a single customer if under the limit
    /// </summary>
    public bool SpawnCustomer()
    {
        if (currentCustomerCount >= maxActiveCustomers)
        {
            Debug.LogWarning($"CustomerSpawner: Cannot spawn, at max capacity ({maxActiveCustomers})");
            return false;
        }
        
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerSpawner: No customer prefab assigned!");
            return false;
        }
        
        // Spawn customer at spawn point
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
        newCustomer.name = $"Customer_{customerNumber}";
        
        // Configure the customer
        CustomerController controller = newCustomer.GetComponent<CustomerController>();
        if (controller != null)
        {
            controller.queueManager = queueManager;
            controller.counterTarget = counterTarget;
            controller.waitingSpots = waitingSpots;
            controller.exitPoint1 = exitPoint1;
            controller.exitPoint2 = exitPoint2;
            controller.customerPrefab = customerPrefab;
            controller.waitingSpotNum = customerNumber % waitingSpots.Length;
            customerNumber++;
        }
        else
        {
            Debug.LogError("CustomerSpawner: Customer prefab is missing CustomerController component!");
            Destroy(newCustomer);
            customerNumber++;
            return false;
        }
        
        // Track the customer
        activeCustomers.Add(newCustomer);
        currentCustomerCount = activeCustomers.Count;
        
        Debug.Log($"CustomerSpawner: Spawned {newCustomer.name} (Total: {currentCustomerCount}/{maxActiveCustomers})");
        return true;
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            // Clean up null references
            activeCustomers.RemoveAll(c => c == null);
            currentCustomerCount = activeCustomers.Count;
            
            // Try to spawn if under limit
            if (currentCustomerCount < maxActiveCustomers)
            {
                SpawnCustomer();
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    /// <summary>
    /// Clear all active customers
    /// </summary>
    public void ClearAllCustomers()
    {
        foreach (GameObject customer in activeCustomers)
        {
            if (customer != null)
            {
                Destroy(customer);
            }
        }
        activeCustomers.Clear();
        currentCustomerCount = 0;
        Debug.Log("CustomerSpawner: Cleared all customers");
    }
    
    /// <summary>
    /// Get the current number of active customers
    /// </summary>
    public int GetActiveCustomerCount()
    {
        activeCustomers.RemoveAll(c => c == null);
        return activeCustomers.Count;
    }
    
    // Visualize spawn point in editor
    void OnDrawGizmos()
    {
        Transform point = spawnPoint != null ? spawnPoint : transform;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(point.position, 0.5f);
        Gizmos.DrawLine(point.position, point.position + point.forward * 1f);
    }
    
    void OnDrawGizmosSelected()
    {
        Transform point = spawnPoint != null ? spawnPoint : transform;
        
        // Draw spawn radius
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(point.position, 0.5f);
    }
}
