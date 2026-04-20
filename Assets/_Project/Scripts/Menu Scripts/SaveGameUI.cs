using UnityEngine;

public class SaveGameUI : MonoBehaviour
{
    public int slotIndex;
    public LoadMenuManager menu;

    public void SetSlot(int index)
    {
        slotIndex = index;
    }

    public void SaveGame()
    {
        SaveManager.SetActiveSlot(slotIndex);

        PlayerPrefs.SetString($"save_{slotIndex}_PlayerName", "Player");
        PlayerPrefs.SetInt($"save_{slotIndex}_Money", 100);
        PlayerPrefs.SetInt($"save_{slotIndex}_GameDayNumber", 1);
        PlayerPrefs.SetInt($"save_{slotIndex}_HasSave", 1);

        PlayerPrefs.Save();

        Debug.Log("Saved to slot: " + slotIndex);

    }
}