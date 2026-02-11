using UnityEngine;

public class PickupCube : MonoBehaviour, Interactable
{
    [Header("Hold Distance")]
    public float holdDistance = 2f;
    public float minHoldDistance = 0.75f;
    public float maxHoldDistance = 4.5f;
    public float scrollSpeed = 1.25f;

    [Header("Follow Sharpness")]
    public float positionSharpness = 25f;
    public float rotationSharpness = 25f;

    [Header("Throw")]
    public float throwVelocityMultiplier = 1.0f;
    public float maxThrowSpeed = 18f;
    public float extraForwardThrow = 1.25f;

    bool isHeld;
    Transform holderCamera;
    float nextDropAllowedTime;

    float snapDisableUntil = 0f;


    Rigidbody rb;
    Collider[] myColliders;
    Collider[] ignoredPlayerColliders;

    // For throw calculation
    Vector3 lastTargetPos;
    Vector3 targetVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myColliders = GetComponentsInChildren<Collider>(true);
    }

    void Update()
    {
        if (!isHeld) return;
        

        // Scroll wheel adjusts hold distance
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            holdDistance += scroll * scrollSpeed;
            holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
        }

        // Drop even if not looking at the object
        if (Time.time >= nextDropAllowedTime && Input.GetKeyDown(KeyCode.E))
        {
            Drop(applyThrow: true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attach") && isHeld && Time.time > snapDisableUntil)
        {
            Drop(false);
            transform.position = other.transform.position;
            ColorPad reference = other.gameObject.GetComponent<ColorPad>();
            if (reference != null) {
                ChatShaderCtrl shdr = GetComponent<ChatShaderCtrl>();
                reference.doughShader = shdr;
            }
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Attach") && onTrigger)
    //    {
    //        onTrigger = false;

    //    }
    //}

    void LateUpdate()
    {
        if (!isHeld || holderCamera == null) return;

        Vector3 targetPos = holderCamera.position + holderCamera.forward * holdDistance;

        // Estimate "hand" velocity for throwing (frame-rate independent)
        if (Time.deltaTime > 0f)
            targetVelocity = (targetPos - lastTargetPos) / Time.deltaTime;
        else
            targetVelocity = Vector3.zero;

        lastTargetPos = targetPos;

        float posT = 1f - Mathf.Exp(-positionSharpness * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, posT);

        Quaternion targetRot = Quaternion.LookRotation(holderCamera.forward, Vector3.up);
        float rotT = 1f - Mathf.Exp(-rotationSharpness * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotT);
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (!isHeld)
        {
            rb.constraints = RigidbodyConstraints.None;
            snapDisableUntil = Time.time + 0.25f;
            PickUp(interactor);
        }
        else
        {
            if (Time.time >= nextDropAllowedTime)
                Drop(applyThrow: true);
        }
    }

    void PickUp(PlayerInteractor interactor)
    {
        // IMPORTANT: rename this if your interactor uses a different camera field
        holderCamera = interactor.playerCamera.transform;

        isHeld = true;
        nextDropAllowedTime = Time.time + 0.15f;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        Debug.Log("Inside pickup");

        // Init throw tracking
        lastTargetPos = holderCamera.position + holderCamera.forward * holdDistance;
        targetVelocity = Vector3.zero;

        IgnorePlayerCollisions(interactor, true);
    }

    void Drop(bool applyThrow)
    {
        isHeld = false;

        rb.isKinematic = false;
        rb.useGravity = true;

        IgnorePlayerCollisions(null, false);

        if (applyThrow)
        {
            // Base throw from how fast the hold point was moving
            Vector3 v = targetVelocity * throwVelocityMultiplier;

            // Add a bit of forward bias so it feels like a "toss"
            if (holderCamera != null)
                v += holderCamera.forward * extraForwardThrow;

            // Clamp so it doesn't become absurd
            if (v.magnitude > maxThrowSpeed)
                v = v.normalized * maxThrowSpeed;

            rb.linearVelocity = v;
        }

        holderCamera = null;
    }

    void IgnorePlayerCollisions(PlayerInteractor interactor, bool ignore)
    {
        if (ignore)
        {
            if (interactor == null) return;

            ignoredPlayerColliders = interactor.GetComponentsInParent<Collider>(true);
            for (int i = 0; i < myColliders.Length; i++)
            {
                for (int j = 0; j < ignoredPlayerColliders.Length; j++)
                {
                    if (myColliders[i] != null && ignoredPlayerColliders[j] != null)
                        Physics.IgnoreCollision(myColliders[i], ignoredPlayerColliders[j], true);
                }
            }
        }
        else
        {
            if (ignoredPlayerColliders == null) return;

            for (int i = 0; i < myColliders.Length; i++)
            {
                for (int j = 0; j < ignoredPlayerColliders.Length; j++)
                {
                    if (myColliders[i] != null && ignoredPlayerColliders[j] != null)
                        Physics.IgnoreCollision(myColliders[i], ignoredPlayerColliders[j], false);
                }
            }

            ignoredPlayerColliders = null;
        }
    }
}
