using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class SaveSlotUI : MonoBehaviour 
{ 
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    private int slotIndex;

    public void Setup(int index)
    {
        slotIndex = index;

        int test = PlayerPrefs.GetInt($"save_{index}_HasSave", -1);
        Debug.Log("Slot " + index + " HasSave = " + test);

        if (test != 1)
        {
            nameText.text = "EMPTY";
            return;
        }

        nameText.text = PlayerPrefs.GetString($"save_{index}_PlayerName", "ERROR");
        moneyText.text = PlayerPrefs.GetInt($"save_{index}_Money", -999).ToString();
        dayText.text = PlayerPrefs.GetInt($"save_{index}_GameDayNumber", -999).ToString();
    }

    public void OnClickSave()
    {
        SaveGameUI save = FindObjectOfType<SaveGameUI>();
        save.SetSlot(slotIndex);
        save.SaveGame();
    }

    public void OnDeleteClick()
    {
        SaveManager.DeleteSave(slotIndex);

        Debug.Log("Deleted slot " + slotIndex);

        Setup(slotIndex);
    }
}