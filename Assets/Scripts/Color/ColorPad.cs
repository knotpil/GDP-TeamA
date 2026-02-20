using UnityEngine;
using UnityEngine.UI;

public class ColorPad : MonoBehaviour
{
    float m_Hue;
    float m_Saturation;
    float m_Value;
    //These are the Sliders that control the values. Remember to attach them in the Inspector window.
    public Slider m_SliderHue, m_SliderSaturation, m_SliderValue;

    public ChatShaderCtrl doughShader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set the maximum and minimum values for the Sliders
        m_SliderHue.maxValue = 1;
        m_SliderSaturation.maxValue = 1;
        m_SliderValue.maxValue = 1;

        m_SliderHue.minValue = 0;
        m_SliderSaturation.minValue = 0;
        m_SliderValue.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (doughShader != null)
        {
            doughShader.furThickness = 1.0f;
            //Create an RGB color from the HSV values from the Sliders
            //Change the Color of your GameObject to the new Color
            doughShader.baseColor = Color.HSVToRGB(m_Hue, m_Saturation, m_Value);
        }
        //These are the Sliders that determine the amount of the hue, saturation and value in the Color
        m_Hue = m_SliderHue.value;
        m_Saturation = m_SliderSaturation.value;
        m_Value = m_SliderValue.value;

    }

    public void AddHue()
    {
        m_SliderHue.value += 0.05f;
    }

    public void RemoveHue()
    {
        m_SliderHue.value -= 0.05f;
    }

    public void AddSat()
    {
        m_SliderSaturation.value += 0.05f;
    }

    public void RemoveSat()
    {
        m_SliderSaturation.value -= 0.05f;
    }

    public void AddVal()
    {
        m_SliderValue.value += 0.05f;
    }

    public void RemoveVal()
    {
        m_SliderValue.value -= 0.05f;
    }

}
