using System;
using UnityEngine;

[Serializable]
public enum CookingMethod
{
    Raw,
    Fried,
    Baked,
    Cut
}

[Serializable]
public class IngredientInstance
{
    public IngredientType type;
    public CookingMethod method;

    public IngredientInstance(IngredientType t, CookingMethod m = CookingMethod.Raw)
    {
        type = t;
        method = m;
    }
}
