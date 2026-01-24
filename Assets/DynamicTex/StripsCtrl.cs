using UnityEngine;

[ExecuteAlways]
public class StripsCtrl : MonoBehaviour
{

    public Material mat;
    public Color baseColor = Color.white;

    public Color stripeColor = Color.black;

    public float stripeWidth = 0.1f;

    public float stripeBlend = 0.01f;

    public float stripeOffset = 0.5f;

    public float stripeCount = 1f;
    
    private Material _instmat;
    
    private Renderer _renderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateStripes();
    }

    void OnValidate()
    {
        UpdateStripes();
    }
    

    private void UpdateStripes()
    {
        _instmat = new Material(mat);
        _instmat.name = mat.name + " (Instance)";
        _renderer = GetComponent<Renderer>();
        _renderer.material = _instmat;
        
        _instmat.SetColor("_BaseColor", baseColor);
        _instmat.SetColor("_StripeColor", stripeColor);
        _instmat.SetFloat("_Count", stripeCount);
        _instmat.SetFloat("_Width", stripeWidth);
        _instmat.SetFloat("_Offset", stripeOffset);
        _instmat.SetFloat("_Smooth", stripeBlend);


    }
}
