using UnityEngine;

public class IngredientDispenser : MonoBehaviour, Interactable
{
    public enum IngredientType { Water, Flour }

    [SerializeField] IngredientType type;
    [SerializeField] int amountPerPress = 1;

    public void Interact(PlayerInteractor interactor)
    {
        if (DoughManager.Instance == null) return;

        if (type == IngredientType.Water)
            DoughManager.Instance.AddWater(amountPerPress);
        else
            DoughManager.Instance.AddFlour(amountPerPress);
    }
}
