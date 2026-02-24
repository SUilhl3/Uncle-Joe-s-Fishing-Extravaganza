using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [HideInInspector]
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
        Debug.Log($"Picked up {type}");
    }

    public void PickUpInstance(IngredientInstance inst)
    {
        if (inst == null) return;
        heldItem = new IngredientInstance(inst.type, inst.method);
        Debug.Log($"Picked up {inst.type} ({inst.method})");
    }

    public void PlaceOnPlate(Cooking cooking)
    {
        if (heldItem == null || cooking == null) return;
        cooking.AddIngredientFromInstance(heldItem);
        Debug.Log($"Placed {heldItem.type} ({heldItem.method}) on plate");
        heldItem = null;
    }

    public void Trash()
    {
        if (heldItem != null)
        {
            Debug.Log($"Trashed {heldItem.type} ({heldItem.method})");
            heldItem = null;
        }
    }

    public void CutHeld()
    {
        if (heldItem == null) return;
        heldItem.method = CookingMethod.Cut;
        Debug.Log($"Cut {heldItem.type}");
    }

    public void BakeHeld()
    {
        if (heldItem == null) return;
        heldItem.method = CookingMethod.Baked;
        Debug.Log($"Started baking {heldItem.type}");
    }

    public void FryHeld()
    {
        if (heldItem == null) return;
        heldItem.method = CookingMethod.Fried;
        Debug.Log($"Started frying {heldItem.type}");
    }
}
