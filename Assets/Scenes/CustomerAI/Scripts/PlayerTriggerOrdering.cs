using UnityEngine;

public class PlayerTriggerOrdering : MonoBehaviour
{
    [Header("Ordering Checkpoint Trigger")]
    public bool orderingCheckpointTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            orderingCheckpointTrigger = true;
            Debug.Log("PlayerTriggerOrdering: Player entered ordering checkpoint");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            orderingCheckpointTrigger = false;
            Debug.Log("PlayerTriggerOrdering: Player left ordering checkpoint");
        }
    }
}
