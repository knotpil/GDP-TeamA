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

    [Header("Pickup Cooldown")]
    public float pickupCooldownAfterDrop = 0.35f;

    [Header("Oven Check")]
    public bool inOven = false;

    const int IGNORE_RAYCAST_LAYER = 2;

    bool isHeld;
    Transform holderCamera;
    float nextDropAllowedTime;
    float nextPickupAllowedTime;

    PlayerInteractor holderInteractor;

    Rigidbody rb;
    Collider[] myColliders;
    Collider[] ignoredPlayerColliders;

    Vector3 lastTargetPos;
    Vector3 targetVelocity;

    Transform[] layerTransforms;
    int[] originalLayers;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myColliders = GetComponentsInChildren<Collider>(true);

        layerTransforms = GetComponentsInChildren<Transform>(true);
        originalLayers = new int[layerTransforms.Length];
        for (int i = 0; i < layerTransforms.Length; i++)
            originalLayers[i] = layerTransforms[i].gameObject.layer;
    }

    void Update()
    {
        if (!isHeld) return;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            holdDistance += scroll * scrollSpeed;
            holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
        }

        if (Time.time >= nextDropAllowedTime && Input.GetKeyDown(KeyCode.E))
        {
            Drop(applyThrow: true);
        }
    }

    void LateUpdate()
    {
        if (!isHeld || holderCamera == null) return;

        Vector3 targetPos = holderCamera.position + holderCamera.forward * holdDistance;

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

    public void ForcePickup(PlayerInteractor interactor)
    {
        if (isHeld) return;
        PickUp(interactor, bypassCooldown: true);
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (!isHeld)
        {
            PickUp(interactor, bypassCooldown: false);
        }
        else
        {
            if (Time.time >= nextDropAllowedTime)
                Drop(applyThrow: true);
        }
    }

    void SetLayerRecursive(int layer)
    {
        for (int i = 0; i < layerTransforms.Length; i++)
            layerTransforms[i].gameObject.layer = layer;
    }

    void RestoreOriginalLayers()
    {
        for (int i = 0; i < layerTransforms.Length; i++)
            layerTransforms[i].gameObject.layer = originalLayers[i];
    }

    void PickUp(PlayerInteractor interactor, bool bypassCooldown)
    {
        if (!bypassCooldown)
        {
            if (Time.time < nextPickupAllowedTime) return;
            if (inOven) return;
        }

        holderCamera = interactor.playerCamera.transform;

        holderInteractor = interactor;
        interactor.holding = this.gameObject;

        isHeld = true;
        nextDropAllowedTime = Time.time + 0.15f;

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        lastTargetPos = holderCamera.position + holderCamera.forward * holdDistance;
        targetVelocity = Vector3.zero;

        IgnorePlayerCollisions(interactor, true);

        // IMPORTANT: let raycasts go through held object
        SetLayerRecursive(IGNORE_RAYCAST_LAYER);
    }

    void Drop(bool applyThrow)
    {
        isHeld = false;

        nextPickupAllowedTime = Time.time + pickupCooldownAfterDrop;

        rb.isKinematic = false;
        rb.useGravity = true;

        IgnorePlayerCollisions(null, false);

        if (applyThrow)
        {
            Vector3 v = targetVelocity * throwVelocityMultiplier;

            if (holderCamera != null)
                v += holderCamera.forward * extraForwardThrow;

            if (v.magnitude > maxThrowSpeed)
                v = v.normalized * maxThrowSpeed;

            rb.linearVelocity = v;
        }

        holderCamera = null;

        if (holderInteractor != null && holderInteractor.holding == this.gameObject)
            holderInteractor.holding = null;
        holderInteractor = null;

        // restore layers so it can be interacted with again when not held
        RestoreOriginalLayers();
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

    public bool IsHeld
    {
        get { return isHeld; }
    }
}
