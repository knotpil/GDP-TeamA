using UnityEngine;

public class OvenInteract : MonoBehaviour, Interactable
{
    //cat currently in the oven
    private GameObject catInOven = null;
    [SerializeField] Transform placement;

    // Update is called once per frame
    void Update()
    {
        if (catInOven != null)
        {
            catInOven.transform.position = placement.position;
            var features = catInOven.GetComponent<CatFeatures>();
            features.personality += Time.deltaTime;
        }
    }

    public void Interact(PlayerInteractor interactor)
    {

        GameObject heldCat = null;
        
        if (interactor.holding != null)
        {
            if (interactor.holding.gameObject.name == "Cat")
            {
                heldCat = interactor.holding.gameObject;
            }
        }

        if (catInOven == null && interactor.holding != null)
        {
            Debug.Log("putting cat in empty oven");
            catInOven = heldCat;

            var dropping = interactor.holding.gameObject.GetComponent<CatInteract>();
            dropping.Interact(interactor);
            dropping.inOven = true;

            interactor.holding = null;
            
        }
        else if (catInOven != null && interactor.holding == null)
        {
            Debug.Log("getting cat from oven");

            interactor.holding = catInOven;

            var getting = interactor.holding.gameObject.GetComponent<CatInteract>();
            getting.inOven = false;
            getting.Interact(interactor);


            catInOven = null;
        }
        else if (catInOven == null && interactor.holding == null)
        {
            Debug.Log("no cat, oven empty");
        }
        else if (catInOven != null && interactor.holding != null)
        {
            Debug.Log("cat in oven, cat in hands");
        }
    }
}
