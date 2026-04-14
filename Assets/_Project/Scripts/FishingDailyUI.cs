using UnityEngine;
using TMPro;

public class FishingDailyUI : MonoBehaviour
{
    public TextMeshProUGUI fishCounterText;

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        int caught = FishingDailyLimitManager.GetFishCaughtToday();
        int max = FishingDailyLimitManager.GetDailyCatchLimit();

        fishCounterText.text = $"Fish Today: {caught} / {max}";
    }
}