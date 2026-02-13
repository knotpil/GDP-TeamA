using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Collider playerTrig;
    public bool checkpointTrigger = false;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerOrderTrigger"))
        {
            checkpointTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerOrderTrigger"))
        {
            checkpointTrigger = false;
        }
    }
}
