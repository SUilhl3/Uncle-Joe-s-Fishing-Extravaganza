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
        buyButton.onClick.AddListener(BuyCurrent);
    }

    public void Show(ShopItemData item, ShopItemHotspot hotspot)
    {
        currentItem = item;
        currentHotspot = hotspot;

        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        descText.text = item.description;
        priceText.text = $"Price: {item.price}";

        buyButton.interactable = CurrencyManager.Instance.CanAfford(item.price);
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
        currentItem = null;
        currentHotspot = null;
    }

    private void BuyCurrent()
    {
        if (currentItem == null || currentHotspot == null) return;

        if (CurrencyManager.Instance.Spend(currentItem.price))
        {
            currentHotspot.OnPurchased();
            Hide();
        }
        else
        {
            buyButton.interactable = false;
        }
    }
}