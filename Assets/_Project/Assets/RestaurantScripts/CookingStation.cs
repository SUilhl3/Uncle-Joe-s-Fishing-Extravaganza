using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum StationType { Fryer, Oven, Cutting }

public enum StationState { Cooking, Ready, Burnt }

public static class CookingStationEvents
{
    public static event Action<StationType, StationState> OnStationStateChanged;

    public static void Emit(StationType t, StationState s)
    {
        OnStationStateChanged?.Invoke(t, s);
    }
}

public class CookingStation : MonoBehaviour, IDropHandler
{
    public StationType stationType;
    public Transform snapTransform;
    DraggableItem currentItem;

    Coroutine cookRoutine;

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        if (go == null) return;
        var item = go.GetComponent<DraggableItem>();
        if (item == null) return;

        item.transform.SetParent(snapTransform ? snapTransform : this.transform, false);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        item.LockToStation(this);
        currentItem = item;
        CookingStationEvents.Emit(stationType, StationState.Cooking);
        if (cookRoutine != null) StopCoroutine(cookRoutine);
        cookRoutine = StartCoroutine(CookingRoutine(item));
    }

    IEnumerator CookingRoutine(DraggableItem item)
    {
        float t = 0f;
        while (t < 3f)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        switch (stationType)
        {
            case StationType.Fryer:
                item.SetMethod(CookingMethod.Fried);
                break;
            case StationType.Oven:
                item.SetMethod(CookingMethod.Baked);
                break;
            case StationType.Cutting:
                item.SetMethod(CookingMethod.Cut);
                break;
        }

        CookingStationEvents.Emit(stationType, StationState.Ready);
        item.UnlockFromStation();

        while (t < 6f)
        {
            t += Time.unscaledDeltaTime;
            if (item == null || item.transform.parent != (snapTransform ? snapTransform : this.transform))
                yield break;
            yield return null;
        }

        if (item != null && item.transform.parent == (snapTransform ? snapTransform : this.transform))
        {
            item.Burn();
            CookingStationEvents.Emit(stationType, StationState.Burnt);
            item.UnlockFromStation();
        }
    }

    public void CancelCooking()
    {
        if (cookRoutine != null)
            StopCoroutine(cookRoutine);
        if (currentItem != null)
            currentItem.UnlockFromStation();
        currentItem = null;
    }
}
