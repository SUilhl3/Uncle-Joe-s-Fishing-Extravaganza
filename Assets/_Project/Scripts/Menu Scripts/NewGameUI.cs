using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        PlayerPrefs.SetInt($"save_{selectedSlot}_PlayerCents", 0);
        PlayerPrefs.SetInt($"save_{selectedSlot}_HasSave", 1);

        PlayerPrefs.Save();

        Debug.Log($"Started new game in slot {selectedSlot} for {playerName}");

        SceneManager.LoadScene("Intro Scene");
    }
}