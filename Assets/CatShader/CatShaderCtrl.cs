using UnityEngine;

[ExecuteAlways]
public class ChatShaderCtrl : MonoBehaviour
{
    [Header("Base Pattern Settings")]
    public Color baseColor = Color.white;
    public Color stripeColor = Color.black;
    [Range(0, 1)] public float stripeWidth = 0.1f;
    [Range(0, 1)] public float stripeBlend = 0.01f;
    [Range(0, 20)] public float stripeOffset = 0.5f;
    [Range(0, 30)] public float stripeCount = 1f;
    public Vector4 stripeDirection = new Vector4(0, 1, 0, 0);
    [Range(0, 10)]public float stripeSpacing = 1.0f;
    [Range(0, 20)] public float waveFrequency = 0.1f;
    [Range(0, 20)] public float waveStrength = 0.1f;
    
    [Header("Spot Settings")]
    public Color spotColor = Color.black;
    [Range(0, 20)] public float spotCount = 5f;
    [Range(0, 1)] public float spotSize = 0.1f;
    [Range(0, 1)] public float spotSmooth = 0.1f;
    [Range(0, 1)] public float spotRandomness = 0.1f;

    [Header("Shell Settings")]
    public Mesh shellMesh;
    public Material shellMaterial;
    [Range(4, 64)] public int shellCount = 16;
    public float shellLength = 0.1f;
    [Range(0.1f, 5f)] public float taperExponent = 1.5f;
    public float furDensity = 100f;
    [Range(0, 1)] public float furThickness = 0.5f;

    public GameObject mesh;
    
    private MaterialPropertyBlock _propBlock;
    
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int StripeColorId = Shader.PropertyToID("_StripeColor");
    private static readonly int CountId = Shader.PropertyToID("_StripeCount");
    private static readonly int WidthId = Shader.PropertyToID("_StripeWidth");
    private static readonly int OffsetId = Shader.PropertyToID("_StripeOffset");
    private static readonly int SmoothId = Shader.PropertyToID("_StripeSmooth");
    private static readonly int DirectionId = Shader.PropertyToID("_StripeDirection");
    private static readonly int SpacingId = Shader.PropertyToID("_StripeSpacing");
    private static readonly int WaveFrequencyId = Shader.PropertyToID("_WaveFrequency");
    private static readonly int WaveStrengthId = Shader.PropertyToID("_WaveStrength");
    
    private static readonly int SpotColorId = Shader.PropertyToID("_SpotColor");
    private static readonly int SpotCountId = Shader.PropertyToID("_SpotCount");
    private static readonly int SpotSizeId = Shader.PropertyToID("_SpotSize");
    private static readonly int SpotThresholdId = Shader.PropertyToID("_SpotThreshold");
    private static readonly int SpotRandomnessId = Shader.PropertyToID("_SpotRandomness");
    
    private static readonly int HeightId = Shader.PropertyToID("_CurrentShellHeight");
    private static readonly int ShellLengthId = Shader.PropertyToID("_ShellLength");
    private static readonly int TaperId = Shader.PropertyToID("_TaperExponent");
    private static readonly int FurDensityId = Shader.PropertyToID("_FurDensity");
    private static readonly int FurThicknessId = Shader.PropertyToID("_FurThickness");

    void OnValidate()
    {
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();
        Renderer renderer = mesh.GetComponent<Renderer>();
        renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor(BaseColorId, baseColor);
        renderer.SetPropertyBlock(_propBlock);
    }

    
    void Update()
    {
        if (shellMesh == null || shellMaterial == null) return;
        DrawShells();
    }

    private void DrawShells()
    {
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();
        
        _propBlock.SetColor(BaseColorId, baseColor);
        _propBlock.SetColor(StripeColorId, stripeColor);
        _propBlock.SetFloat(CountId, stripeCount);
        _propBlock.SetFloat(WidthId, stripeWidth);
        _propBlock.SetFloat(OffsetId, stripeOffset);
        _propBlock.SetFloat(SmoothId, stripeBlend);
        _propBlock.SetVector(DirectionId, stripeDirection);
        _propBlock.SetFloat(SpacingId, stripeSpacing);
        _propBlock.SetFloat(WaveFrequencyId, waveFrequency);
        _propBlock.SetFloat(WaveStrengthId, waveStrength);
        
        _propBlock.SetColor(SpotColorId, spotColor);
        _propBlock.SetFloat(SpotCountId, spotCount);
        _propBlock.SetFloat(SpotSizeId, spotSize);
        _propBlock.SetFloat(SpotThresholdId, spotSmooth);
        _propBlock.SetFloat(SpotRandomnessId, spotRandomness);
        
        _propBlock.SetFloat(ShellLengthId, shellLength);
        _propBlock.SetFloat(TaperId, taperExponent);
        _propBlock.SetFloat(FurDensityId, furDensity);
        _propBlock.SetFloat(FurThicknessId, furThickness);
        
        for (int i = 0; i < shellCount; i++)
        {
            float h = (float)i / shellCount;
            _propBlock.SetFloat(HeightId, h);

            Graphics.DrawMesh(
                shellMesh, 
                transform.localToWorldMatrix, 
                shellMaterial, 
                gameObject.layer, 
                null, 0, _propBlock
            );
        }
    }
}