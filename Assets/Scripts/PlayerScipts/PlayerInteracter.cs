using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 3f;

    public GameObject holding = null;

    public TextMeshProUGUI interactionText;

    const int IGNORE_RAYCAST_LAYER = 2;

    void Start()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        interactableCheck();

        if (Input.GetKeyDown(KeyCode.E))
            TryInteract();
    }

    void interactableCheck()
    {
        if (interactionText == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        int mask = ~(1 << IGNORE_RAYCAST_LAYER);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, mask, QueryTriggerInteraction.Ignore))
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = "Press E to interact";
                return;
            }
        }

        interactionText.gameObject.SetActive(false);
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        int mask = ~(1 << IGNORE_RAYCAST_LAYER);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, mask, QueryTriggerInteraction.Ignore))
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
            if (interactable != null)
                interactable.Interact(this);
        }
    }
}
