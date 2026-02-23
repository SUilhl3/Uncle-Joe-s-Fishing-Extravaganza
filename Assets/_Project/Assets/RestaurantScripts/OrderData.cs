using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOrder", menuName = "Cooking/Order")]
public class OrderData : ScriptableObject
{
    public List<IngredientType> requiredIngredients;
    
    public List<CookingMethod> requiredCookingMethods = new();
    
    public string orderDialogue;
    public float basePrice = 1f;
    [Range(0, 100)]
    public int rarityPercentage = 10;
}
