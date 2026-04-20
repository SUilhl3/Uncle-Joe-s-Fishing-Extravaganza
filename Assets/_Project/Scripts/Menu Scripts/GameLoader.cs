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
        int money = SaveManager.GetMoney();
        int day = SaveManager.GetDayNumber();

        Debug.Log($"Loaded: {playerName}, ${money}, Day {day}");

        var inventory = CatchInventoryManager.Export();

        FishingDailyLimitManager.LoadFromSave(new SaveData
        {
            hasChest = FishingDailyLimitManager.HasChest(),
            hasBait = FishingDailyLimitManager.HasBait(),
            hasBait2 = FishingDailyLimitManager.HasBait2(),
            fishCaughtToday = FishingDailyLimitManager.GetFishCaughtToday()
        });
    }
}