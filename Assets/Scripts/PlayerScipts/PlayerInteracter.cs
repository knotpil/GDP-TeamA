using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 3f;

    //reference to what player is holding
    public GameObject holding = null;

    public TextMeshProUGUI interactionText;


    void Start()
    {
        //making the text hidden while not interaction range
        if(interactionText != null) 
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {

        interactableCheck(); // keep checking for interactable obj

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void interactableCheck()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        //raycast ignores triggers
        if (Physics.Raycast(ray, out hit, interactRange, ~0, QueryTriggerInteraction.Ignore))
        {


            // check if obj has the interactable script
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();

           // if it is interactable show interactionText
            if (interactable != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = "Press E to interact";
                return;
            }
        }

        // making sure that if nothing is hit, text is hidden!
        interactionText.gameObject.SetActive(false);
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, ~0, QueryTriggerInteraction.Ignore))
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }
}
