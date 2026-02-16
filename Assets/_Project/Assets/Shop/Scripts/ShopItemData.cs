using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    [TextArea] 
    public string description;
    public Sprite icon;
    public int price;
}