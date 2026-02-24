using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItemData : ScriptableObject
{
    public string itemName;

    [TextArea]
    public string description;

    public Sprite icon;

    [Tooltip("Price in cents (100 = $1.00)")]
    public int priceInCents;
}