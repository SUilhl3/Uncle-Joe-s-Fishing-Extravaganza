using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOrder", menuName = "Cooking/Order")]
public class OrderData : ScriptableObject
{
    public List<IngredientType> requiredIngredients;
    public string orderDialogue;
}
