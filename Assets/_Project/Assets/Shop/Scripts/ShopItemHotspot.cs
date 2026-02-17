using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemHotspot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private ShopItemData itemData;
    [SerializeField] private GameObject highlight;
    [SerializeField] private ShopTooltipUI tooltip;

    private bool purchased;

    private void Start()
    {
        if (highlight != null) highlight.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (purchased) return;
        if (highlight != null) highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlight != null) highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (purchased) return;

        if (eventData.button != PointerEventData.InputButton.Left) return;

        tooltip.Show(itemData, this);
    }

    public void OnPurchased()
    {
        purchased = true;
        if (highlight != null) highlight.SetActive(false);

        gameObject.SetActive(false);
    }
}