using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public float itemValue;
}
