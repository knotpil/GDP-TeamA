using UnityEngine;

[ExecuteAlways]
public class ApplySelectionShader : MonoBehaviour
{
    public Renderer mesh;
    
    [Range(1, 2)] public float outlineThickness= 1.1f;
    public Color outlineColor = Color.limeGreen;
    
    private MaterialPropertyBlock materialProperty;
    
    private static readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    private static readonly int OutlineColorID = Shader.PropertyToID("_OutlineColor");
    
    void OnValidate()
    {
        if (mesh == null) return;
        
        if (materialProperty == null) materialProperty = new MaterialPropertyBlock();
        
        materialProperty.SetFloat(OutlineThicknessID, outlineThickness);
        materialProperty.SetColor(OutlineColorID, outlineColor);
        
        mesh.SetPropertyBlock(materialProperty);
    }
}
