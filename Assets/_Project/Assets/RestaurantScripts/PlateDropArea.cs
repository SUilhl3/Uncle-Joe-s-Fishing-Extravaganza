using UnityEngine;
using UnityEngine.EventSystems;

public class PlateDropArea : MonoBehaviour, IDropHandler
{
    public Cooking cooking;

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        if (go == null) return;
        var item = go.GetComponent<DraggableItem>();
        if (item == null) return;
        if (cooking != null && item.instance != null)
            cooking.AddIngredientFromInstance(item.instance);

    }
}
