using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBuyerRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text ownedText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button sellButton;

    private TreasureBuyerCatalogEntry currentEntry;
    private int currentPrice;

    public void Setup(TreasureBuyerCatalogEntry entry, int price)
    {
        currentEntry = entry;
        currentPrice = price;

        if (sellButton != null)
        {
            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(SellOne);
        }

        Refresh();
    }

    public void Refresh()
    {
        if (currentEntry == null) return;

        int owned = CatchInventoryManager.GetCount(currentEntry.id);

        if (nameText != null) nameText.text = currentEntry.displayName;
        if (ownedText != null) ownedText.text = "Owned: " + owned;
        if (priceText != null) priceText.text = "Buyer Price: $" + (currentPrice / 100f).ToString("F2");

        if (sellButton != null)
            sellButton.interactable = owned > 0;
    }

    private void SellOne()
    {
        if (currentEntry == null) return;

        bool removed = CatchInventoryManager.RemoveItem(currentEntry.id, 1);
        if (!removed) return;

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCents(currentPrice);
        }

        Refresh();
    }
}