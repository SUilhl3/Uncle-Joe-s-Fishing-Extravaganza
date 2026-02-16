using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemHotspot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private ShopItemData itemData;
    [SerializeField] private GameObject highlight;
    [SerializeField] private ShopTooltipUI tooltip;
    [SerializeField] private bool followMouse = true;

    private bool purchased;

    private void Start()
    {
        if (highlight != null) highlight.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (purchased) return;

        if (highlight != null) highlight.SetActive(true);
        tooltip.Show(itemData, this);

        if (followMouse) tooltip.SetScreenPosition(eventData.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!followMouse || purchased) return;
        tooltip.SetScreenPosition(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlight != null) highlight.SetActive(false);
        tooltip.Hide();
    }

    public void OnPurchased()
    {
        purchased = true;
        if (highlight != null) highlight.SetActive(false);

        gameObject.SetActive(false);

    }
}