using UnityEngine;
using TMPro;
using System;

public class DayUI : MonoBehaviour
{
    public TextMeshProUGUI dayText;

    private DateTime startingDate = new DateTime(2026, 1, 1);

    void Start()
    {
        UpdateDayText();
    }

    public void UpdateDayText()
    {
        DateTime currentDate = SaveManager.GetCurrentGameDate();
        int dayNumber = (currentDate - startingDate).Days + 1;

        dayText.text = $"Day {dayNumber}";
    }
}