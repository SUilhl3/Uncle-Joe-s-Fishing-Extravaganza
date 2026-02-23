using UnityEngine;
using UnityEngine.EventSystems;

public class TrashDropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        if (go == null) return;
    }
}
