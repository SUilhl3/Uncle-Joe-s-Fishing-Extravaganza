using UnityEngine;

public static class FishingDailyLimitManager
{
    private const string FishCaughtKeyPrefix = "FishCaught_";
    private const string ChestPurchasedKey = "ChestPurchased";
    private const string BaitPurchasedKey = "BaitPurchased";
    private const string Bait2PurchasedKey = "Bait2Purchased";

    private const int BaseDailyLimit = 10;
    private const int ChestBonus = 10;
    private const int BaitBonus = 5;
    private const int Bait2Bonus = 5;

    private static string TodayKey =>
        FishCaughtKeyPrefix + SaveManager.GetCurrentGameDate().ToString("yyyyMMdd");

    public static int GetFishCaughtToday()
    {
        return PlayerPrefs.GetInt(TodayKey, 0);
    }

    public static int GetDailyCatchLimit()
    {
        int bonus = 0;

        if (HasChest()) bonus += ChestBonus;
        if (HasBait()) bonus += BaitBonus;
        if (HasBait2()) bonus += Bait2Bonus;

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

    public static void PurchaseBait()
    {
        PlayerPrefs.SetInt(BaitPurchasedKey, 1);
        PlayerPrefs.Save();
    }

    public static void PurchaseBait2()
    {
        PlayerPrefs.SetInt(Bait2PurchasedKey, 1);
        PlayerPrefs.Save();
    }

    public static bool HasChest()
    {
        return PlayerPrefs.GetInt(ChestPurchasedKey, 0) == 1;
    }

    public static bool HasBait()
    {
        return PlayerPrefs.GetInt(BaitPurchasedKey, 0) == 1;
    }

    public static bool HasBait2()
    {
        return PlayerPrefs.GetInt(Bait2PurchasedKey, 0) == 1;
    }
}