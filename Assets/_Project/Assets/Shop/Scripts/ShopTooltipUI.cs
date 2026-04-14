using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopTooltipUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    private ShopItemData currentItem;
    private ShopItemHotspot currentHotspot;

    private void Awake()
    {
        Hide();

        if (buyButton != null)
            buyButton.onClick.AddListener(BuyCurrent);
    }

    public void Show(ShopItemData item, ShopItemHotspot hotspot)
    {
        if (item == null || hotspot == null)
            return;

        if (root != null && root.activeSelf && currentHotspot == hotspot)
        {
            Hide();
            return;
        }

        currentItem = item;
        currentHotspot = hotspot;

        if (iconImage != null) iconImage.sprite = item.icon;
        if (nameText != null) nameText.text = item.itemName;
        if (descText != null) descText.text = item.description;

        if (priceText != null)
            priceText.text = $"Price: ${(item.priceInCents / 100f):F2}";

        if (CurrencyManager.Instance != null)
        {
            if (buyButton != null)
                buyButton.interactable = CurrencyManager.Instance.CanAfford(item.priceInCents);
        }
        else
        {
            Debug.LogWarning("CurrencyManager.Instance is null. Make sure CurrencyManager exists in this scene.");
            if (buyButton != null)
                buyButton.interactable = false;
        }

        if (root != null)
            root.SetActive(true);
    }

    public void Hide()
    {
        if (root != null) root.SetActive(false);
        currentItem = null;
        currentHotspot = null;
    }

    private void BuyCurrent()
    {
        if (currentItem == null || currentHotspot == null)
            return;

        if (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("CurrencyManager.Instance is null. Make sure CurrencyManager exists in this scene.");
            if (buyButton != null) buyButton.interactable = false;
            return;
        }

        if (CurrencyManager.Instance.Spend(currentItem.priceInCents))
        {
            if (currentItem.itemName == "Chest")
            {
                FishingDailyLimitManager.PurchaseChest();
            }
            else if (currentItem.itemName == "Bait")
            {
                FishingDailyLimitManager.PurchaseBait();
            }
            else if (currentItem.itemName == "Bait 2")
            {
                FishingDailyLimitManager.PurchaseBait2();
            }

            currentHotspot.OnPurchased();
            Hide();
        }
        else
        {
            if (buyButton != null) buyButton.interactable = false;
        }
    }
}