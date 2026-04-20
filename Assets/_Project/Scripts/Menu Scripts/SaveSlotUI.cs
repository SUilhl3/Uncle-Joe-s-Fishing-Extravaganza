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
            SaveManager.SetActiveSlot(index);
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
        SaveManager.SetActiveSlot(slotIndex);
        SceneManager.LoadScene("GameScene"); 
    }

    public void OnDeleteClick()
    {
        SaveManager.DeleteSave(slotIndex);
        Setup(slotIndex);
    }
}