using UnityEngine;

public class ExampleUse : MonoBehaviour
{
    private ColorController controller;
    private float darken = 0;
    void Start()
    {
        controller = GetComponent<ColorController>();
    }

    void FixedUpdate()
    {
        controller.SetColor(controller.baseColor, darken);
        darken += .01f;
        if(darken > 1)
        {
            darken = 0;
        }
    }
}
