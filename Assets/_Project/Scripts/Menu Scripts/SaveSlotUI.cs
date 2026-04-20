using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class SaveSlotUI : MonoBehaviour 
{ 
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    private int slotIndex;
    
    public void Setup(int index) {
        slotIndex = index;
        
        if (SaveManager.SaveExists(index))
        {
            string tempName = PlayerPrefs.GetString($"save_{index}_PlayerName", "Player");
            int tempMoney = PlayerPrefs.GetInt($"save_{index}_Money", 0);
            int tempDay = PlayerPrefs.GetInt($"save_{index}_GameDayNumber", 1);
            nameText.text = SaveManager.GetPlayerName();
            moneyText.text = "$" + SaveManager.GetMoney();
            dayText.text = "Day " + SaveManager.GetDayNumber();
        } 
        else
        { 
            nameText.text = "Empty Slot";
            moneyText.text = "";
            dayText.text = "";
        }
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
        Setup(slotIndex);
    }
}