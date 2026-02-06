using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuImageChanger : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public Image changeImage;   // image that changes
    public Sprite defaultSprite;  // default
    public Sprite hoverSprite;  // sprite for this button

    void Start()
    {
        if (hoverSprite != null && defaultSprite != null)
            changeImage.sprite = defaultSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        changeImage.sprite = hoverSprite;
    }

    // for keyboard
    public void OnSelect(BaseEventData eventData)
    {
        changeImage.sprite = hoverSprite;
    }
}

