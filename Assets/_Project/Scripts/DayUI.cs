using UnityEngine;
using TMPro;

public class DayUI : MonoBehaviour
{
    public TextMeshProUGUI dayText;

    void Start()
    {
        UpdateDayText();
    }

    void OnEnable()
    {
        UpdateDayText();
    }

    public void UpdateDayText()
    {
        if (dayText == null)
            return;

        int dayNumber = SaveManager.GetDayNumber();
        dayText.text = $"Day {dayNumber}";
    }
}