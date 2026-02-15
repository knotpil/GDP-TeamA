using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [Header("Checkpoint References")]
    public Collider orderingCheckpointCollider;
    public Collider waitingCheckpointCollider;
    
    private Collider playerTrig;
    public bool checkpointTrigger = false;  // Legacy - keep for backwards compatibility
    public bool isInOrderingCheckpoint = false;
    public bool isInWaitingCheckpoint = false;
    
    void Start()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger)
            {
                playerTrig = collider;
                break;
            }
        }
        
        // Debug checkpoint references
        //Debug.Log($"PlayerTrigger: Ordering checkpoint assigned: {orderingCheckpointCollider != null}");
        //Debug.Log($"PlayerTrigger: Waiting checkpoint assigned: {waitingCheckpointCollider != null}");
        
        //if (orderingCheckpointCollider != null)
        //{
        //    Debug.Log($"PlayerTrigger: Ordering checkpoint is: {orderingCheckpointCollider.gameObject.name}");
        //}
        //if (waitingCheckpointCollider != null)
        //{
        //    Debug.Log($"PlayerTrigger: Waiting checkpoint is: {waitingCheckpointCollider.gameObject.name}");
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"PlayerTrigger: OnTriggerEnter - Collided with: {other.gameObject.name}, Tag: {other.tag}");
        
        if (other.CompareTag("PlayerOrderTrigger"))
        {
            checkpointTrigger = true;
        }
        
        // Check if entering the ordering checkpoint
        if (orderingCheckpointCollider != null && other == orderingCheckpointCollider)
        {
            isInOrderingCheckpoint = true;
            //Debug.Log("PlayerTrigger: Player entered Ordering checkpoint");
        }
        
        // Check if entering the waiting checkpoint
        if (waitingCheckpointCollider != null && other == waitingCheckpointCollider)
        {
            isInWaitingCheckpoint = true;
            //Debug.Log("PlayerTrigger: Player entered Waiting checkpoint");
        }
        
        // Debug info about checkpoint references
        //if (waitingCheckpointCollider == null)
        //{
        //    Debug.LogWarning("PlayerTrigger: waitingCheckpointCollider reference is NULL! Assign it in the Inspector.");
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"PlayerTrigger: OnTriggerExit - Left collision with: {other.gameObject.name}");
        
        if (other.CompareTag("PlayerOrderTrigger"))
        {
            checkpointTrigger = false;
        }
        
        // Check if leaving the ordering checkpoint
        if (orderingCheckpointCollider != null && other == orderingCheckpointCollider)
        {
            isInOrderingCheckpoint = false;
            //Debug.Log("PlayerTrigger: Player left Ordering checkpoint");
        }
        
        // Check if leaving the waiting checkpoint
        if (waitingCheckpointCollider != null && other == waitingCheckpointCollider)
        {
            isInWaitingCheckpoint = false;
            //Debug.Log("PlayerTrigger: Player left Waiting checkpoint");
        }
    }
}
