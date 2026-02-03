using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    public Texture2D cursor1;
    public Texture2D cursor2;
    public Texture2D cursor3;
    public Texture2D cursor4;

    private Vector2 hotspot = Vector2.zero;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadSavedCursor();
    }

    public void SetCursor1()
    {
        Cursor.SetCursor(cursor1, hotspot, CursorMode.Auto);
        PlayerPrefs.SetInt("CursorChoice", 1);
    }

    public void SetCursor2()
    {
        Cursor.SetCursor(cursor2, hotspot, CursorMode.Auto);
        PlayerPrefs.SetInt("CursorChoice", 2);
    }

    public void SetCursor3()
    {
        Cursor.SetCursor(cursor3, hotspot, CursorMode.Auto);
        PlayerPrefs.SetInt("CursorChoice", 3);
    }

    public void SetCursor4()
    {
        Cursor.SetCursor(cursor4, hotspot, CursorMode.Auto);
        PlayerPrefs.SetInt("CursorChoice", 4);
    }

    void LoadSavedCursor()
    {
        int saved = PlayerPrefs.GetInt("CursorChoice", 1);

        if (saved == 1) SetCursor1();
        else if (saved == 2) SetCursor2();
        else if (saved == 3) SetCursor3();
        else if (saved == 4) SetCursor4();
    }
}
