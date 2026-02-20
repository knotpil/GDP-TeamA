using UnityEngine;

/// <summary>
/// Simple helper script. Put this on the PLAYER CHECKPOINT (where player stands to serve).
/// This GameObject should have a trigger collider.
/// </summary>
public class PlayerCheckpointTrigger : MonoBehaviour
{
    public bool playerIsHere = false;
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"PlayerCheckpoint {gameObject.name}: OnTriggerEnter with {other.gameObject.name}, Tag: {other.tag}");
        
        if (other.CompareTag("Player"))
        {
            playerIsHere = true;
            //Debug.Log($"PlayerCheckpoint {gameObject.name}: Player entered");
        }
        //else
        //{
        //    Debug.Log($"PlayerCheckpoint {gameObject.name}: Not a Player tag, ignoring");
        //}
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsHere = false;
            //Debug.Log($"PlayerCheckpoint {gameObject.name}: Player left");
        }
    }
}
