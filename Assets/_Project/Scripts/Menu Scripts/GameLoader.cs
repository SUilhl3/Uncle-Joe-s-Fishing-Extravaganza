using UnityEngine;

public class GameLoader : MonoBehaviour
{
    void Start()
    {
        LoadGame();
    }

    void LoadGame()
    {
        string playerName = SaveManager.GetPlayerName();
        int day = SaveManager.GetDayNumber();

        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.Load();

        int money = CurrencyManager.Instance != null ? CurrencyManager.Instance.Cents : 0;

        Debug.Log($"Loaded: {playerName}, ${(money / 100f):F2}, Day {day}");

        var inventory = CatchInventoryManager.Export();

        FishingDailyLimitManager.LoadFromSave(new SaveData
        {
            playerName = playerName,
            money = money,
            day = day,
            inventory = inventory,
            hasChest = FishingDailyLimitManager.HasChest(),
            hasBait = FishingDailyLimitManager.HasBait(),
            hasBait2 = FishingDailyLimitManager.HasBait2(),
            fishCaughtToday = FishingDailyLimitManager.GetFishCaughtToday(),
            slotIndex = SaveManager.GetCurrentSlot()
        });
    }
}