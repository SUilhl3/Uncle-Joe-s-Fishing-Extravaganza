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

        string playerName = SaveManager.GetPlayerName();
        int day = SaveManager.GetDayNumber();
        int cents = CurrencyManager.Instance != null ? CurrencyManager.Instance.Cents : 0;

        PlayerPrefs.SetString($"save_{slotIndex}_PlayerName", playerName);
        PlayerPrefs.SetInt($"save_{slotIndex}_PlayerCents", cents);
        PlayerPrefs.SetInt($"save_{slotIndex}_GameDayNumber", day);
        PlayerPrefs.SetInt($"save_{slotIndex}_HasSave", 1);

        PlayerPrefs.Save();

        Debug.Log($"Saved slot {slotIndex}: {playerName}, ${(cents / 100f):F2}, Day {day}");
    }
}