using UnityEngine;

public static class FishingDailyLimitManager
{
    private const int BaseDailyLimit = 10;
    private const int ChestBonus = 10;
    private const int BaitBonus = 5;
    private const int Bait2Bonus = 5;

    private static string ChestPurchasedKey => SaveManager.SlotKey("ChestPurchased");
    private static string BaitPurchasedKey => SaveManager.SlotKey("BaitPurchased");
    private static string Bait2PurchasedKey => SaveManager.SlotKey("Bait2Purchased");

    private static string TodayKey =>
        SaveManager.SlotKey("FishCaught_" + SaveManager.GetCurrentGameDate().ToString("yyyyMMdd"));

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

        if (DailyEffectManager.Instance != null)
        {
            if (DailyEffectManager.Instance.HasEffect(DailyEffectType.AbundantWaters))
                bonus += DailyEffectManager.Instance.GetIntValue(DailyEffectType.AbundantWaters);

            if (DailyEffectManager.Instance.HasEffect(DailyEffectType.LowEnergy))
                bonus -= DailyEffectManager.Instance.GetIntValue(DailyEffectType.LowEnergy);
        }

        return Mathf.Max(1, BaseDailyLimit + bonus);
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

    public static void LoadFromSave(SaveData data)
    {
        PlayerPrefs.SetInt(ChestPurchasedKey, data.hasChest ? 1 : 0);
        PlayerPrefs.SetInt(BaitPurchasedKey, data.hasBait ? 1 : 0);
        PlayerPrefs.SetInt(Bait2PurchasedKey, data.hasBait2 ? 1 : 0);
        PlayerPrefs.SetInt(TodayKey, data.fishCaughtToday);
        PlayerPrefs.Save();
    }
}