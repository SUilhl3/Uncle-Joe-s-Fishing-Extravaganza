using UnityEngine;

public class SaveGameUI : MonoBehaviour
{
    public int slotIndex;

    public void SaveGame()
    {
        SaveManager.SetActiveSlot(slotIndex);

        SaveManager.SetPlayerName("Player");
        SaveManager.SetMoney(100);
        SaveManager.SetCurrentGameDate(System.DateTime.Now);

        PlayerPrefs.SetInt($"save_{slotIndex}_HasSave", 1);

        PlayerPrefs.Save();

        Debug.Log("Saved to slot: " + slotIndex);
    }
}