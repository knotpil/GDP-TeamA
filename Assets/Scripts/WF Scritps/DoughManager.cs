using UnityEngine;

public class DoughManager : MonoBehaviour
{
    public static DoughManager Instance { get; private set; }

    [Header("Prefab / Spawn")]
    [SerializeField] GameObject doughPrefab;
    [SerializeField] Transform doughSpawn;

    [Header("Sizing")]
    [SerializeField] float baseSize = 0.25f;
    [SerializeField] float sizePerUnit = 0.10f;
    [SerializeField] float flattenedThickness = 0.18f;

    [Header("Materials")]
    [SerializeField] Material doughCorrectMat;  // water == flour (tan sphere)
    [SerializeField] Material waterMat;         // water > flour (blue flattened sphere)
    [SerializeField] Material flourMat;         // flour > water (tan flattened square)

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

    public void AddWater(int amount)
    {
        water += Mathf.Max(0, amount);
        UpdateDoughVisual();
    }

    public void AddFlour(int amount)
    {
        flour += Mathf.Max(0, amount);
        UpdateDoughVisual();
    }

    void EnsureDoughExists()
    {
        if (doughObj != null) return;

        if (doughPrefab == null)
        {
            Debug.LogError("DoughManager: doughPrefab is not assigned in the Inspector.");
            return;
        }

        Vector3 pos = doughSpawn != null ? doughSpawn.position : Vector3.up;

        doughObj = Instantiate(doughPrefab, pos, Quaternion.identity);
        doughObj.name = "Dough";

        float size = baseSize;
        doughObj.transform.localScale = Vector3.one * size;

        ApplyStateVisuals();
    }

    void UpdateDoughVisual()
    {
        EnsureDoughExists();
        if (doughObj == null) return;

        ApplyStateVisuals();
    }

    void ApplyStateVisuals()
    {
        if (doughObj == null) return;

        Renderer r = doughObj.GetComponentInChildren<Renderer>();
        if (r == null)
        {
            Debug.LogError("DoughManager: Dough prefab has no Renderer on it or its children.");
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
