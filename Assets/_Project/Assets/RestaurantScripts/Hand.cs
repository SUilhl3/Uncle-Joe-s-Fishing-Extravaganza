using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public IngredientInstance heldItem;
    public Button trashButton;

    void Start()
    {
        if (trashButton != null)
            trashButton.onClick.AddListener(Trash);
    }

    public bool IsHolding() => heldItem != null;

    public void PickUp(IngredientType type)
    {
        heldItem = new IngredientInstance(type, CookingMethod.Raw);
    }

    public void PickUpInstance(IngredientInstance inst)
    {
        if (inst == null) return;
        heldItem = new IngredientInstance(inst.type, inst.method);
    }

    public void PlaceOnPlate(Cooking cooking)
    {
        if (heldItem == null || cooking == null) return;
        cooking.AddIngredientFromInstance(heldItem);
        heldItem = null;
    }

    public void Trash()
    {
        heldItem = null;
    }

    public void CutHeld()
    {
        if (heldItem == null) return;
        heldItem.method = CookingMethod.Cut;
    }

    public void BakeHeld()
    {
        if (heldItem == null) return;
        heldItem.method = CookingMethod.Baked;
    }

    public void FryHeld()
    {
        if (heldItem == null) return;
        heldItem.method = CookingMethod.Fried;
    }
}
