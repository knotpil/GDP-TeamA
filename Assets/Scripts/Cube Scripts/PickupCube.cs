using UnityEngine;

public class PickupCube : MonoBehaviour, Interactable
{
    public float holdDistance = 2f;
    public float followSpeed = 20f;

    bool isHeld;
    Transform holderCamera;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isHeld && holderCamera != null)
        {
            Vector3 targetPos = holderCamera.position + holderCamera.forward * holdDistance;
            Vector3 newPos = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
        }
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (!isHeld)
        {
            holderCamera = interactor.playerCamera.transform;
            isHeld = true;
            interactor.holding = this.gameObject;

            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.freezeRotation = true;
            Debug.Log("Holding" + this.gameObject.name);
        }
        else
        {
            isHeld = false;
            holderCamera = null;

            rb.useGravity = true;
            rb.freezeRotation = false;
        }
    }
}
