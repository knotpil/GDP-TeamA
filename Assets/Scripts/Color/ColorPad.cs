using UnityEngine;

public class ColorPad : MonoBehaviour
{
    public ChatShaderCtrl doughShader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (doughShader != null)
        {
            doughShader.furThickness = 1.0f;
        }
    }

    public void AddRed()
    {
        doughShader.baseColor.r += 0.05f;
    }

    public void AddBlue()
    {
        doughShader.baseColor.b += 0.05f;
    }

    public void AddGreen()
    {
        doughShader.baseColor.g += 0.05f;
    }

    public void RemoveRed()
    {
        doughShader.baseColor.r -= 0.05f;
    }

    public void RemoveBlue()
    {
        doughShader.baseColor.b -= 0.05f;
    }

    public void RemoveGreen()
    {
        doughShader.baseColor.g -= 0.05f;
    }

}
