using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;

    private int slotIndex;

    public void Setup(int index)
    {
        slotIndex = index;

        int hasSave = PlayerPrefs.GetInt($"save_{index}_HasSave", 0);

        if (hasSave != 1)
        {
            nameText.text = "EMPTY";
            moneyText.text = "";
            dayText.text = "";
            return;
        }

        // Name
        string name = PlayerPrefs.GetString($"save_{index}_PlayerName", "Player");

        // Money (from CurrencyManager system)
        int cents = PlayerPrefs.GetInt($"save_{index}_PlayerCents", 0);
        float dollars = cents / 100f;

        // Day
        int day = PlayerPrefs.GetInt($"save_{index}_GameDayNumber", 1);

        nameText.text = name;
        moneyText.text = $"${dollars:F2}";
        dayText.text = $"Day {day}";
    }

    public void OnClickLoad()
    {
        int hasSave = PlayerPrefs.GetInt($"save_{slotIndex}_HasSave", 0);

        if (hasSave != 1)
        {
            Debug.Log("No save in this slot.");
            return;
        }

        SaveManager.SetActiveSlot(slotIndex);
        Debug.Log("Loading slot " + slotIndex);

        SceneManager.LoadScene("Map");
    }

    public void OnDeleteClick()
    {
        SaveManager.DeleteSave(slotIndex);
        Debug.Log("Deleted slot " + slotIndex);
        Setup(slotIndex);
    }
}