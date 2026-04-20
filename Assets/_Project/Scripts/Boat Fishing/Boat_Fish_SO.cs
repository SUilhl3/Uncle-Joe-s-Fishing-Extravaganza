using UnityEngine;

[CreateAssetMenu(fileName = "BoatFish", menuName = "Boat Fishing/BoatFish")]
public class Boat_Fish_SO : ScriptableObject
{
    public string fishName;
    public string description;
    public Sprite fishSprite;
    public int value;
    public float probabilityOfCatch;
    public ItemRarity rarity;

    [Header("Inventory / Restaurant Data")]
    public FishSize fishSize = FishSize.Medium;
}