using UnityEngine;

public class CursorButton : MonoBehaviour
{
    public int cursorID;

    public void SelectCursor()
    {
        if (CursorManager.Instance == null)
        {
            Debug.LogWarning("CursorManager not found!");
            return;
        }

        if (cursorID == 1) CursorManager.Instance.SetCursor1();
        else if (cursorID == 2) CursorManager.Instance.SetCursor2();
        else if (cursorID == 3) CursorManager.Instance.SetCursor3();
        else if (cursorID == 4) CursorManager.Instance.SetCursor4();
    }
}

