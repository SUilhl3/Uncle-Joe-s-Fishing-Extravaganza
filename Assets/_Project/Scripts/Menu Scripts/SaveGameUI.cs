using UnityEngine;

public class SaveGameUI : MonoBehaviour
{
    public void SaveGame()
    {
        PlayerPrefs.Save();

        Debug.Log("Game Saved!");
    }
}