using UnityEngine;

public class ColorController : MonoBehaviour
{
    public Color baseColor = Color.white;
    [Range(0, 1)]
    public float darkenModifier = 0.0f;

    public Material baseMaterial;
    private Material instancedMaterial;
    private Renderer objRenderer;

    void OnValidate()
    {
        UpdateColorValues();
    }

    void Start()
    {
        InitializeRenderer();
        UpdateColorValues();
    }

    private void InitializeRenderer()
    {
        if (objRenderer == null) objRenderer = GetComponent<Renderer>();

        if (baseMaterial != null && instancedMaterial == null)
        {
            instancedMaterial = new Material(baseMaterial);
            objRenderer.material = instancedMaterial;
        }
    }

    public void UpdateColorValues()
    {
        SetColor(baseColor, darkenModifier);
    }

    public void SetColor(Color c, float darkMod)
    {
        if (objRenderer == null) objRenderer = GetComponent<Renderer>();

        Color finalColor = Color.Lerp(c, Color.black, darkMod);

        if (instancedMaterial != null)
        {
            instancedMaterial.color = finalColor;
        }
        else if (objRenderer != null)
        {
            objRenderer.sharedMaterial.color = finalColor;
        }
    }
}

