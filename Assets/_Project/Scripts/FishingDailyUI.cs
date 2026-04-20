using UnityEngine;
using TMPro;

public class FishingDailyUI : MonoBehaviour
{
    public TextMeshProUGUI fishCounterText;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        int caught = FishingDailyLimitManager.GetFishCaughtToday();
        int max = FishingDailyLimitManager.GetDailyCatchLimit();

        fishCounterText.text = $"Catches Today: {caught} / {max}";
    }
}