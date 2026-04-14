using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public float itemValue;
    public float itemWeight;
    public float probabilityOfCatch;
    public ItemRarity itemRarity;

    [Header("Inventory / Restaurant Data")]
    public bool isFish = false;
    public FishSize fishSize = FishSize.None;
}