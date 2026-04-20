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

        bool exists = PlayerPrefs.GetInt($"save_{index}_HasSave", 0) == 1;

        if (!exists)
        {
            nameText.text = "Empty Slot";
            moneyText.text = "";
            dayText.text = "";
            return;
        }

        nameText.text = PlayerPrefs.GetString($"save_{index}_PlayerName", "Player");
        moneyText.text = "$" + PlayerPrefs.GetInt($"save_{index}_Money", 0);
        dayText.text = "Day " + PlayerPrefs.GetInt($"save_{index}_GameDayNumber", 1);
    }

    public void OnClick()
    {
        if (!SaveManager.SaveExists(slotIndex))
            return;

        SaveManager.SetActiveSlot(slotIndex);

        SceneManager.LoadScene("GameScene");
    }

    public void OnDeleteClick()
    {
        SaveManager.DeleteSave(slotIndex);

        Debug.Log("Deleted slot " + slotIndex);

        Setup(slotIndex);
    }
}