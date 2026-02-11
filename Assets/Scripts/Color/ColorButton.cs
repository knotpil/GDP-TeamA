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
        if (gameObject.name == "Red")
        {
            colorPad.AddRed();
        }
        else if (gameObject.name == "Blue")
        {
            colorPad.AddBlue();
        }
        else if (gameObject.name == "Green")
        {
            colorPad.AddGreen();
        }
        else if (gameObject.name == "-Red")
        {
            colorPad.RemoveRed();
        }
        else if (gameObject.name == "-Blue")
        {
            colorPad.RemoveBlue();
        }
        else if (gameObject.name == "-Green")
        {
            colorPad.RemoveGreen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
