using TMPro;
using UnityEngine;

public class NewGameUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public int selectedSlot = 0;

    public void SetSelectedSlot(int slot)
    {
        selectedSlot = slot;
    }

    public void StartNewGame()
    {
        SaveManager.SetActiveSlot(selectedSlot);

        string playerName = string.IsNullOrWhiteSpace(nameInput.text) ? "Player" : nameInput.text.Trim();

        SaveManager.SetPlayerName(playerName);
        PlayerPrefs.SetInt($"save_{selectedSlot}_GameDayNumber", 1);
        PlayerPrefs.SetString($"save_{selectedSlot}_GameDate", System.DateTime.Now.Date.ToString("yyyyMMdd"));

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.SetCents(0);
        }
        else
        {
            PlayerPrefs.SetInt($"save_{selectedSlot}_PlayerCents", 0);
        }

        SaveManager.MarkSlotAsUsed();
        PlayerPrefs.Save();

        Debug.Log($"Started new game in slot {selectedSlot} for {playerName}");
    }
}