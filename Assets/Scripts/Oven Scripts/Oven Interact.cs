using UnityEngine;

public class OvenInteract : MonoBehaviour
{

    //currently only used to check if something is an oven



    ////the object currently in the oven
    //private GameObject ovenItem = null;
    ////[SerializeField] Transform placement;

    //void Update()
    //{
    //    //if we got something in the oven, increase its ovenTime var
    //    if (ovenItem != null)
    //    {
    //        var timer = ovenItem.GetComponent<CatFeatures>();
    //        if(timer) timer.ovenTime += Time.deltaTime;
    //    }
    //}

    ////public void Interact(PlayerInteractor interactor)
    //{
    //    GameObject heldItem = null;

    //    //interactor has a reference to holding something
    //    if (interactor.holding != null)
    //    {
    //        heldItem = interactor.holding.gameObject;
    //    }
    
    //    //oven empty, player holding something
    //    if (ovenItem == null && interactor.holding != null)
    //    {
    //        ovenItem = heldItem;

    //        Rigidbody rb = ovenItem.GetComponent<Rigidbody>();
    //        if (rb)
    //        {
    //            rb.isKinematic = true;
    //            rb.linearVelocity = Vector3.zero;
    //            rb.angularVelocity = Vector3.zero;
    //        }

    //        ovenItem.transform.position = placement.position;
    //        ovenItem.transform.rotation = placement.rotation;

    //        //calling interact script to make the object act "dropped"
    //        var dropping = interactor.holding.gameObject.GetComponent<PickupCube>();
    //        dropping.Interact(interactor);
    //        dropping.inOven = true;

    //        interactor.holding = null;

    //    }
    //    //oven full, hands empty, taking from oven
    //    else if (ovenItem != null && interactor.holding == null)
    //    {
    //        interactor.holding = ovenItem;
    //        Rigidbody rb = ovenItem.GetComponent<Rigidbody>();
    //        if (rb)
    //        {
    //            rb.isKinematic = false;
    //        }

    //        //calling interact script to make the object act "picked upo"
    //        var getting = interactor.holding.gameObject.GetComponent<PickupCube>();
    //        getting.inOven = false;
    //        getting.Interact(interactor);


    //        ovenItem = null;
    //    }

    //    //oven empy, hands empty
    //    else if (ovenItem == null && interactor.holding == null)
    //    {
    //        Debug.Log("no cat, oven empty");
    //    }

    //    //oven full, hands full.
    //    else if (ovenItem != null && interactor.holding != null)
    //    {
    //        Debug.Log("cat in oven, cat in hands");
    //    }
        
    //}
}
