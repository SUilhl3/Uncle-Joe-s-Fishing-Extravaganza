using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public List<IngredientInstance> currentIngredients = new();

    public Hand hand;

    public void AddSmallFish()
    {
        AddFishToHand(FishSize.Small, IngredientType.SmallFish);
    }

    public void AddMediumFish()
    {
        AddFishToHand(FishSize.Medium, IngredientType.MediumFish);
    }

    public void AddLargeFish()
    {
        AddFishToHand(FishSize.Large, IngredientType.LargeFish);
    }

    public void AddCheese()
    {
        AddIngredientToHand(IngredientType.Cheese);
    }

    public void AddLettuce()
    {
        AddIngredientToHand(IngredientType.Lettuce);
    }

    public void AddOnion()
    {
        AddIngredientToHand(IngredientType.Onion);
    }

    public void AddLemon()
    {
        AddIngredientToHand(IngredientType.Lemon);
    }

    void AddFishToHand(FishSize fishSize, IngredientType ingredientType)
    {
        if (hand == null) return;
        if (hand.IsHolding()) return;

        if (CatchInventoryManager.GetFishCountBySize(fishSize) <= 0)
        {
            Debug.Log("No " + fishSize + " fish in inventory.");
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowMessage($"No {fishSize} fish in inventory.");
            return;
        }

        bool removed = CatchInventoryManager.ConsumeFish(fishSize, 1);
        if (!removed)
        {
            Debug.Log("Failed to remove " + fishSize + " fish from inventory.");
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowMessage($"Failed to remove {fishSize} fish from inventory.");
            return;
        }

        hand.PickUp(ingredientType);
        Debug.Log("Picked up: " + ingredientType);
        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowMessage($"{ingredientType} added to hand");
    }

    void AddIngredientToHand(IngredientType ingredient)
    {
        if (hand == null) return;
        if (hand.IsHolding()) return;

        hand.PickUp(ingredient);
        Debug.Log("Picked up: " + ingredient);
        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowMessage($"{ingredient} added to hand");
    }

    public void AddIngredientFromInstance(IngredientInstance instance)
    {
        if (instance == null) return;
        currentIngredients.Add(new IngredientInstance(instance.type, instance.method));
        Debug.Log("Added instance to plate: " + instance.type + " (" + instance.method + ")");
        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowMessage($"{instance.type} ({instance.method}) added to plate");
    }

    public void ClearPlate()
    {
        currentIngredients.Clear();
    }

    public void AddToPlateFromHand()
    {
        if (hand == null) return;
        hand.PlaceOnPlate(this);
        Debug.Log("Added ingredient from hand to plate");
    }

    public void ApplyCookingToPlate(int index, CookingMethod method)
    {
        if (index < 0 || index >= currentIngredients.Count) return;
        currentIngredients[index].method = method;
    }

    public void RemoveFromPlateAt(int index)
    {
        if (index < 0 || index >= currentIngredients.Count) return;
        currentIngredients.RemoveAt(index);
    }

    public void RemoveLastFromPlate()
    {
        if (currentIngredients.Count == 0) return;
        currentIngredients.RemoveAt(currentIngredients.Count - 1);
    }

    public void TrashHeld()
    {
        if (hand == null) return;
        hand.Trash();
        Debug.Log("Trashed held ingredient");
    }

    public void TrashLast()
    {
        RemoveLastFromPlate();
        Debug.Log("Trashed last ingredient from plate");
    }

    public void TrashAt(int index)
    {
        RemoveFromPlateAt(index);
        Debug.Log("Trashed ingredient at index: " + index);
    }

    public void TrashAll()
    {
        ClearPlate();
        Debug.Log("Trashed all ingredients");
    }

    public void FryHeld()
    {
        if (hand == null) return;
        hand.FryHeld();
        Debug.Log("Applied Fry to held ingredient");
    }

    public void BakeHeld()
    {
        if (hand == null) return;
        hand.BakeHeld();
        Debug.Log("Applied Bake to held ingredient");
    }

    public void CutHeld()
    {
        if (hand == null) return;
        hand.CutHeld();
        Debug.Log("Applied Cut to held ingredient");
    }

    public void FryAt(int index)
    {
        ApplyCookingToPlate(index, CookingMethod.Fried);
    }

    public void BakeAt(int index)
    {
        ApplyCookingToPlate(index, CookingMethod.Baked);
    }

    public void CutAt(int index)
    {
        ApplyCookingToPlate(index, CookingMethod.Cut);
    }
}
