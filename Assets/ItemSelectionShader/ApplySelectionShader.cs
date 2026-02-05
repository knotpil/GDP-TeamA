using UnityEngine;

public class ApplySelectionShader : MonoBehaviour
{
    public Renderer mesh;
    public Material material;
    
    [Range(1, 2)] public float outlineThickness= 1.1f;
    public Color outlineColor = Color.limeGreen;
    
    private MaterialPropertyBlock materialProperty;
    
    private static readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    private static readonly int OutlineColorID = Shader.PropertyToID("_OutlineColor");
    
    void Start()
    {
        if (mesh == null) return;
        
        if (materialProperty == null) materialProperty = new MaterialPropertyBlock();
        
        var materials = mesh.sharedMaterials;

        var newMaterials = new Material[materials.Length + 1];
        for(int i = 0; i < materials.Length; ++i)
        {
            newMaterials[i] =  materials[i];
        }
        
        newMaterials[materials.Length] = Instantiate(material);
        
        mesh.sharedMaterials = newMaterials;
        
        mesh.GetPropertyBlock(materialProperty);
        
        materialProperty.SetFloat(OutlineThicknessID, outlineThickness);
        materialProperty.SetColor(OutlineColorID, outlineColor);
        
        mesh.SetPropertyBlock(materialProperty);
    }
}
