using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Customer customer;
    public Cooking cooking;

    public void ServeOrder()
    {
        bool success = CheckOrder();
        customer.ReactToOrder(success);
        cooking.ClearPlate();
    }

    bool CheckOrder()
    {
        var required = customer.currentOrder.requiredIngredients;
        var player = cooking.currentIngredients;

        if (required.Count != player.Count)
            return false;

        foreach (var ingredient in required)
        {
            if (!player.Contains(ingredient))
                return false;
        }

        return true;
    }
}
