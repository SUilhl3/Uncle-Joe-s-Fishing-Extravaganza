using UnityEngine;

public static class FishingDailyLimitManager
{
    private const string FishCaughtKeyPrefix = "FishCaught_";
    private const string ChestPurchasedKey = "ChestPurchased";
    private const int BaseDailyLimit = 10;
    private const int ChestBonus = 10;

    private static string TodayKey =>
        FishCaughtKeyPrefix + SaveManager.GetCurrentGameDate().ToString("yyyyMMdd");

    public static int GetFishCaughtToday()
    {
        return PlayerPrefs.GetInt(TodayKey, 0);
    }

    public static int GetDailyCatchLimit()
    {
        int bonus = HasChest() ? ChestBonus : 0;
        return BaseDailyLimit + bonus;
    }

    public static bool HasReachedLimit()
    {
        return GetFishCaughtToday() >= GetDailyCatchLimit();
    }

    public static bool TryRegisterCatch()
    {
        if (HasReachedLimit())
            return false;

        int caughtToday = GetFishCaughtToday();
        PlayerPrefs.SetInt(TodayKey, caughtToday + 1);
        PlayerPrefs.Save();
        return true;
    }

    public static int GetRemainingCatches()
    {
        return Mathf.Max(0, GetDailyCatchLimit() - GetFishCaughtToday());
    }

    public static void PurchaseChest()
    {
        PlayerPrefs.SetInt(ChestPurchasedKey, 1);
        PlayerPrefs.Save();
    }

    public static bool HasChest()
    {
        return PlayerPrefs.GetInt(ChestPurchasedKey, 0) == 1;
    }
}