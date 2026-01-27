using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 3f;

    //reference to what player is holding
    public GameObject holding = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }
}
