using TMPro;
using UnityEngine;

public class NewGameUI : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void StartNewGame()
    {
        SaveManager.SetPlayerName(nameInput.text);
        SaveManager.SetMoney(0);
        SaveManager.SetActiveSlot(0);
        PlayerPrefs.SetInt("save_0_GameDayNumber", 1);
        PlayerPrefs.Save();
    }
}