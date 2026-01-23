using UnityEngine;

public class ColorToggleCube : MonoBehaviour, Interactable
{
    bool isRed = true;
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        SetColor();
    }

    public void Interact(PlayerInteractor interactor)
    {
        isRed = !isRed;
        SetColor();
    }

    void SetColor()
    {
        if (rend != null)
            rend.material.color = isRed ? Color.red : Color.blue;
    }
}
