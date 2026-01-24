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

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int StripeColorId = Shader.PropertyToID("_StripeColor");
    private static readonly int CountId = Shader.PropertyToID("_Count");
    private static readonly int WidthId = Shader.PropertyToID("_Width");
    private static readonly int OffsetId = Shader.PropertyToID("_Offset");
    private static readonly int SmoothId = Shader.PropertyToID("_Smooth");

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
        
        _renderer.SetPropertyBlock(_propBlock);
    }
}