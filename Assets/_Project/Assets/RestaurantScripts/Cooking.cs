using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public List<IngredientType> currentIngredients = new();

    public void AddSmallFish()
    {
        AddIngredient(IngredientType.SmallFish);
    }

    public void AddMediumFish()
    {
        AddIngredient(IngredientType.MediumFish);
    }

      public void AddLargeFish()
    {
        AddIngredient(IngredientType.LargeFish);
    }

    public void AddCheese()
    {
        AddIngredient(IngredientType.Cheese);
    }

    public void AddLettuce()
    {
        AddIngredient(IngredientType.Lettuce);
    }

    public void AddOnion()
    {
        AddIngredient(IngredientType.Onion);
    }

        public void AddLemon()
    {
        AddIngredient(IngredientType.Lemon);
    }

    void AddIngredient(IngredientType ingredient)
    {
        currentIngredients.Add(ingredient);
        Debug.Log("Added: " + ingredient);
    }

    public void ClearPlate()
    {
        currentIngredients.Clear();
    }
}
