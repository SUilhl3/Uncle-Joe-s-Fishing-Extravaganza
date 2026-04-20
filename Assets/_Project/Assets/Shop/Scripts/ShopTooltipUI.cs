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
                SaveManager.SetSlotInt("TacklePurchased", 1);
            }
            else if (currentItem.itemName == "Teleporter")
            {
                SaveManager.SetSlotInt("TeleporterPurchased", 1);
            }
            else if (currentItem.itemName == "Crab")
            {
                SaveManager.SetSlotInt("CrabPurchased", 1);
            }
            else if (currentItem.itemName == "Elephant Fish")
            {
                SaveManager.SetSlotInt("ElephantFishPurchased", 1);
            }
            else if (currentItem.itemName == "Unknown Fish")
            {
                SaveManager.SetSlotInt("UnknownFishPurchased", 1);
            }
            else if (currentItem.itemName == "Super Sea Cucumber")
            {
                SaveManager.SetSlotInt("SuperSeaCucumberPurchased", 1);
            }
            else if (currentItem.itemName == "Plate Fish")
            {
                SaveManager.SetSlotInt("PlateFishPurchased", 1);
            }
            else if (currentItem.itemName == "Sea Bun")
            {
                SaveManager.SetSlotInt("SeaBunPurchased", 1);
            }
            else if (currentItem.itemName == "Duck")
            {
                SaveManager.SetSlotInt("DuckPurchased", 1);
            }
            else if (currentItem.itemName == "Duck 2")
            {
                SaveManager.SetSlotInt("Duck2Purchased", 1);
            }
            else if (currentItem.itemName == "Toy")
            {
                SaveManager.SetSlotInt("ToyPurchased", 1);
            }
            else if (currentItem.itemName == "Dragon")
            {
                SaveManager.SetSlotInt("DragonPurchased", 1);
            }
            else if (currentItem.itemName == "Dino")
            {
                SaveManager.SetSlotInt("DinoPurchased", 1);
            }
            else if (currentItem.itemName == "Frog")
            {
                SaveManager.SetSlotInt("FrogPurchased", 1);
            }
            else if (currentItem.itemName == "Radio")
            {
                SaveManager.SetSlotInt("RadioPurchased", 1);
            }
            else if (currentItem.itemName == "Patrick House")
            {
                SaveManager.SetSlotInt("PatrickHousePurchased", 1);
            }
            else if (currentItem.itemName == "Lamp")
            {
                SaveManager.SetSlotInt("LampPurchased", 1);
            }
            else if (currentItem.itemName == "Miss Puff")
            {
                SaveManager.SetSlotInt("MissPuffPurchased", 1);
            }
            else if (currentItem.itemName == "Pink")
            {
                SaveManager.SetSlotInt("PinkPurchased", 1);
            }
            else if (currentItem.itemName == "0-0")
            {
                SaveManager.SetSlotInt("ZeroZeroPurchased", 1);
            }
            else if (currentItem.itemName == "Egg")
            {
                SaveManager.SetSlotInt("EggPurchased", 1);
            }
            else if (currentItem.itemName == "Can")
            {
                SaveManager.SetSlotInt("CanPurchased", 1);
            }
            else if (currentItem.itemName == "Gum")
            {
                SaveManager.SetSlotInt("GumPurchased", 1);
            }
            else if (currentItem.itemName == "Jug")
            {
                SaveManager.SetSlotInt("JugPurchased", 1);
            }
            else if (currentItem.itemName == "Dish")
            {
                SaveManager.SetSlotInt("DishPurchased", 1);
            }
            else if (currentItem.itemName == "Dish 2")
            {
                SaveManager.SetSlotInt("Dish2Purchased", 1);
            }
            else if (currentItem.itemName == "Dish 3")
            {
                SaveManager.SetSlotInt("Dish3Purchased", 1);
            }
            else if (currentItem.itemName == "Dish 4")
            {
                SaveManager.SetSlotInt("Dish4Purchased", 1);
            }
            else if (currentItem.itemName == "Uhhh yes")
            {
                SaveManager.SetSlotInt("UhhhYesPurchased", 1);
            }
            else if (currentItem.itemName == "Painting")
            {
                SaveManager.SetSlotInt("PaintingPurchased", 1);
            }
            else if (currentItem.itemName == "Stack of Books")
            {
                SaveManager.SetSlotInt("StackOfBooksPurchased", 1);
            }
            else if (currentItem.itemName == "Chicken Nugget")
            {
                SaveManager.SetSlotInt("ChickenNuggetPurchased", 1);
            }
            else if (currentItem.itemName == "Eye")
            {
                SaveManager.SetSlotInt("EyePurchased", 1);
            }
            else if (currentItem.itemName == "Void")
            {
                SaveManager.SetSlotInt("VoidPurchased", 1);
            }
            else if (currentItem.itemName == "Mirror")
            {
                SaveManager.SetSlotInt("MirrorPurchased", 1);
            }
            else if (currentItem.itemName == "Also Mirror")
            {
                SaveManager.SetSlotInt("AlsoMirrorPurchased", 1);
            }
            else if (currentItem.itemName == "Small Plate")
            {
                SaveManager.SetSlotInt("SmallPlatePurchased", 1);
            }
            else if (currentItem.itemName == "Sea Turtle Egg")
            {
                SaveManager.SetSlotInt("SeaTurtleEggPurchased", 1);
            }
            else if (currentItem.itemName == "Egg 2")
            {
                SaveManager.SetSlotInt("Egg2Purchased", 1);
            }

            currentHotspot.OnPurchased();
            Hide();

            FindFirstObjectByType<FishingDailyUI>()?.Refresh();
        }
        else
        {
            if (buyButton != null) buyButton.interactable = false;
        }
    }
}