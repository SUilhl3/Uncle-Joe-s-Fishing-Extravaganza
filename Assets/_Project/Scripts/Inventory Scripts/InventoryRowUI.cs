using TMPro;
using UnityEngine;

public class InventoryRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private TMP_Text typeText;

    public void Setup(CatchInventoryEntry entry)
    {
        nameText.text = entry.displayName;
        countText.text = "x" + entry.count;

        if (entry.isFish)
            typeText.text = entry.fishSize.ToString() + " Fish";
        else
            typeText.text = "Item";
    }
}