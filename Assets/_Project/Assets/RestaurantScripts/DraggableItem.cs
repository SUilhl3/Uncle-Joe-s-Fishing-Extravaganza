using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public IngredientInstance instance;
    public Sprite rawSprite;
    public Sprite friedSprite;
    public Sprite bakedSprite;
    public Sprite cutSprite;
    public Sprite burntSprite;

    Image image;
    CanvasGroup canvasGroup;
    RectTransform rect;
    bool locked = false;
    CookingStation lockedStation = null;
    Vector2 originalPosition;
    bool pickedUp = false;
    Color originalColor;

    void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        if (rect != null)
            originalPosition = rect.anchoredPosition;
        if (image != null)
            originalColor = image.color;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (image == null) return;
        if (instance == null)
        {
            image.sprite = rawSprite;
            return;
        }

        switch (instance.method)
        {
            case CookingMethod.Fried:
                image.sprite = friedSprite ? friedSprite : rawSprite;
                break;
            case CookingMethod.Baked:
                image.sprite = bakedSprite ? bakedSprite : rawSprite;
                break;
            case CookingMethod.Cut:
                image.sprite = cutSprite ? cutSprite : rawSprite;
                break;
            default:
                image.sprite = rawSprite;
                break;
        }

        if (instance != null && instance.method == CookingMethod.Raw && burntSprite != null && image.sprite == null)
            image.sprite = rawSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (locked) return;
        pickedUp = true;
        SetPickedUpVisual(true);
    }

    void SetPickedUpVisual(bool picked)
    {
        if (image == null) return;
        if (picked)
        {
            Color brightColor = originalColor;
            brightColor.a = 1f;
            image.color = brightColor;
        }
        else
        {
            image.color = originalColor;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!pickedUp || locked) return;
        if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!pickedUp || locked) return;
        rect.anchoredPosition += eventData.delta / (GetCanvasScale());
    }

    float GetCanvasScale()
    {
        var c = GetComponentInParent<Canvas>();
        return c ? c.scaleFactor : 1f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (locked) return;
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;
        pickedUp = false;
        SetPickedUpVisual(false);
            rect.anchoredPosition = originalPosition;
    }

    public void LockToStation(CookingStation station)
    {
        locked = true;
        lockedStation = station;
        if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
    }

    public void UnlockFromStation()
    {
        locked = false;
        lockedStation = null;
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;
    }

    public bool IsLocked() => locked;

    public void SetMethod(CookingMethod m)
    {
        if (instance == null) instance = new IngredientInstance(IngredientType.SmallFish, m);
        else instance.method = m;
        UpdateSprite();
    }

    public void Burn()
    {
        if (instance == null) return;
        instance.method = CookingMethod.Raw; 
        if (image != null && burntSprite != null)
            image.sprite = burntSprite;
    }
}
