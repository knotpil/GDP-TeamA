using UnityEngine;

[ExecuteAlways]
public class ChatShaderCtrl : MonoBehaviour
{
    public Color baseColor = Color.white;
    public Color stripeColor = Color.black;
    [Range(0, 1)] public float stripeWidth = 0.1f;
    [Range(0, 1)] public float stripeBlend = 0.01f;
    public float stripeOffset = 0.5f;
    public float stripeCount = 1f;
    public Vector4 stripeDirection = new Vector4(0, 1, 0, 0);
    public float stripeSpacing = 1.0f;
    
    public Color spotColor = Color.black;
    public float spotCount = 5f;
    [Range(0, 1)] public float spotSize = 0.1f;
    [Range(0, 1)] public float spotSmooth = 0.1f;
    [Range(0, 1)] public float spotRandomness = 0.1f;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int StripeColorId = Shader.PropertyToID("_StripeColor");
    private static readonly int CountId = Shader.PropertyToID("_StripeCount");
    private static readonly int WidthId = Shader.PropertyToID("_StripeWidth");
    private static readonly int OffsetId = Shader.PropertyToID("_StripeOffset");
    private static readonly int SmoothId = Shader.PropertyToID("_StripeSmooth");
    private static readonly int DirectionId = Shader.PropertyToID("_StripeDirection");
    private static readonly int SpacingId = Shader.PropertyToID("_StripeSpacing");
    
    private static readonly int SpotColorId = Shader.PropertyToID("_SpotColor");
    private static readonly int SpotCountId = Shader.PropertyToID("_SpotCount");
    private static readonly int SpotSizeId = Shader.PropertyToID("_SpotSize");
    private static readonly int SpotThresholdId = Shader.PropertyToID("_SpotThreshold");
    private static readonly int SpotRandomnessId = Shader.PropertyToID("_SpotRandomness");

    void OnEnable()
    {
        UpdateStripes(stripeColor, stripeWidth, stripeOffset, stripeBlend, stripeCount);

    }

    void OnValidate()
    {
        UpdateStripes(stripeColor, stripeWidth, stripeOffset, stripeBlend, stripeCount);

    }

    private void UpdateStripes(Color strpColor, float strpWidth, float strpOffset, float strpBlend, float strpCount)
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();
        
        _renderer.GetPropertyBlock(_propBlock);
        
        _propBlock.SetColor(BaseColorId, baseColor);
        _propBlock.SetColor(StripeColorId, strpColor);
        _propBlock.SetFloat(CountId, strpCount);
        _propBlock.SetFloat(WidthId, strpWidth);
        _propBlock.SetFloat(OffsetId, strpOffset);
        _propBlock.SetFloat(SmoothId, strpBlend);
        _propBlock.SetVector(DirectionId, stripeDirection);
        _propBlock.SetFloat(SpacingId, stripeSpacing);
        
        _propBlock.SetColor(SpotColorId, spotColor);
        _propBlock.SetFloat(SpotCountId, spotCount);
        _propBlock.SetFloat(SpotSizeId, spotSize);
        _propBlock.SetFloat(SpotThresholdId, spotSmooth);
        _propBlock.SetFloat(SpotRandomnessId, spotRandomness);
        
        _renderer.SetPropertyBlock(_propBlock);
    }
    
}