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
            else if (currentItem.itemName == "Tackle")
            {
                PlayerPrefs.SetInt("TacklePurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Teleporter")
            {
                PlayerPrefs.SetInt("TeleporterPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Crab")
            {
                PlayerPrefs.SetInt("CrabPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Elephant Fish")
            {
                PlayerPrefs.SetInt("ElephantFishPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Unknown Fish")
            {
                PlayerPrefs.SetInt("UnknownFishPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Super Sea Cucumber")
            {
                PlayerPrefs.SetInt("SuperSeaCucumberPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Plate Fish")
            {
                PlayerPrefs.SetInt("PlateFishPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Sea Bun")
            {
                PlayerPrefs.SetInt("SeaBunPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Duck")
            {
                PlayerPrefs.SetInt("DuckPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Duck 2")
            {
                PlayerPrefs.SetInt("Duck2Purchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Toy")
            {
                PlayerPrefs.SetInt("ToyPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Dragon")
            {
                PlayerPrefs.SetInt("DragonPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Dino")
            {
                PlayerPrefs.SetInt("DinoPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Frog")
            {
                PlayerPrefs.SetInt("FrogPurchased", 1);
                PlayerPrefs.Save();
            }
            else if (currentItem.itemName == "Radio")
            {
                PlayerPrefs.SetInt("RadioPurchased", 1);
                PlayerPrefs.Save();
            }

            currentHotspot.OnPurchased();
            Hide();

            FindObjectOfType<FishingDailyUI>()?.Refresh();
        }
        else
        {
            if (buyButton != null) buyButton.interactable = false;
        }
    }
}