using UnityEngine;

public class ColorButton : MonoBehaviour, Interactable
{
    GameObject colorPadObj;
    public ColorPad colorPad = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorPadObj = GameObject.Find("ColorPad");
        colorPad = colorPadObj.gameObject.GetComponent<ColorPad>();
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (gameObject.name == "+Hue")
        {
            colorPad.AddHue();
        }
        else if (gameObject.name == "-Hue")
        {
            colorPad.RemoveHue();
        }
        else if (gameObject.name == "+Sat")
        {
            colorPad.AddSat();
        }
        else if (gameObject.name == "-Sat")
        {
            colorPad.RemoveSat();
        }
        else if (gameObject.name == "+Val")
        {
            colorPad.AddVal();
        }
        else if (gameObject.name == "-Val")
        {
            colorPad.RemoveVal();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
