using UnityEngine;

public class PlayerTriggerWaiting : MonoBehaviour
{
    [Header("Waiting Checkpoint Trigger")]
    public bool waitingCheckpointTrigger = false;

    private void Start()
    {
        //Debug.Log($"PlayerTriggerWaiting: Initialized on {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"PlayerTriggerWaiting: OnTriggerEnter - Collided with: {other.gameObject.name}, Tag: {other.tag}");
        
        if (other.CompareTag("Player"))
        {
            waitingCheckpointTrigger = true;
            //Debug.Log("PlayerTriggerWaiting: Player entered waiting checkpoint");
        }
        //else
        //{
        //    Debug.LogWarning($"PlayerTriggerWaiting: Object entered but tag is '{other.tag}', expected 'Player'");
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"PlayerTriggerWaiting: OnTriggerExit - Left collision with: {other.gameObject.name}");
        
        if (other.CompareTag("Player"))
        {
            waitingCheckpointTrigger = false;
            //Debug.Log("PlayerTriggerWaiting: Player left waiting checkpoint");
        }
    }
}
