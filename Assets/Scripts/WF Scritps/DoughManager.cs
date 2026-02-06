using UnityEngine;

public class DoughManager : MonoBehaviour
{
    public static DoughManager Instance { get; private set; }

    [Header("Prefab / Spawn")]
    [SerializeField] GameObject doughPrefab;
    [SerializeField] Transform doughSpawn;

    [Header("Release Detection")]
    [SerializeField] float removedFromSpawnDistance = 0.6f;

    [Header("Sizing")]
    [SerializeField] float baseSize = 0.25f;
    [SerializeField] float sizePerUnit = 0.10f;
    [SerializeField] float flattenedThickness = 0.18f;

    [Header("Materials")]
    [SerializeField] Material doughCorrectMat;
    [SerializeField] Material waterMat;
    [SerializeField] Material flourMat;

    int water;
    int flour;

    GameObject doughObj;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (doughObj == null || doughSpawn == null) return;

        bool taken = Vector3.Distance(doughObj.transform.position, doughSpawn.position) > removedFromSpawnDistance;

        var pickup = doughObj.GetComponent<PickupCube>();
        if (pickup != null && pickup.IsHeld) taken = true;

        if (taken)
        {
            ReleaseCurrentDough(); // IMPORTANT: no respawn here
        }
    }

    public void AddWater(int amount)
    {
        if (amount <= 0) return;

        EnsureDoughExists();   // spawn ONLY when the dispenser is pressed
        water += amount;
        ApplyStateVisuals();
    }

    public void AddFlour(int amount)
    {
        if (amount <= 0) return;

        EnsureDoughExists();   // spawn ONLY when the dispenser is pressed
        flour += amount;
        ApplyStateVisuals();
    }

    void ReleaseCurrentDough()
    {
        // stop dispensers from modifying the carried/removed dough
        doughObj = null;

        // reset the station state so the next press starts a fresh dough
        water = 0;
        flour = 0;
    }

    void EnsureDoughExists()
    {
        if (doughObj != null) return;

        if (doughPrefab == null)
        {
            Debug.LogError("DoughManager: doughPrefab is not assigned.");
            return;
        }

        Vector3 pos = doughSpawn != null ? doughSpawn.position : Vector3.up;
        doughObj = Instantiate(doughPrefab, pos, Quaternion.identity);
        doughObj.name = "Dough";

        float size = baseSize;
        doughObj.transform.localScale = Vector3.one * size;
    }

    void ApplyStateVisuals()
    {
        if (doughObj == null) return;

        Renderer r = doughObj.GetComponentInChildren<Renderer>();
        if (r == null)
        {
            Debug.LogError("DoughManager: Dough prefab has no Renderer (on it or children).");
            return;
        }

        if (water == flour)
        {
            if (doughCorrectMat != null) r.material = doughCorrectMat;

            float size = baseSize + (water * sizePerUnit);
            doughObj.transform.localScale = new Vector3(size, size, size);
        }
        else if (water > flour)
        {
            if (waterMat != null) r.material = waterMat;

            float size = baseSize + (water * sizePerUnit);
            doughObj.transform.localScale = new Vector3(size, flattenedThickness, size);
        }
        else
        {
            if (flourMat != null) r.material = flourMat;

            float size = baseSize + (flour * sizePerUnit);
            doughObj.transform.localScale = new Vector3(size, flattenedThickness, size);
        }
    }
}
