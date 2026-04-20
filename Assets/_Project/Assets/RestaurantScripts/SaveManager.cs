using System;
using UnityEngine;

public static class SaveManager
{
    private static int currentSlot = 0;

    public static void SetActiveSlot(int slot)
    {
        currentSlot = slot;
    }

    private static string Prefix(string key) => $"save_{currentSlot}_{key}";
    const string GameDateKey = "GameDate";
    const string GameDayKey = "GameDayNumber";
    static string DailyKey(DateTime date) => Prefix($"TotalEarned_{date:yyyyMMdd}");

    public static void MarkSlotAsUsed()
    {
        PlayerPrefs.SetInt(Prefix("HasSave"), 1);
        PlayerPrefs.Save();
    }

    public static DateTime GetCurrentGameDate()
    {
        string v = PlayerPrefs.GetString(Prefix(GameDateKey), "");
        if (string.IsNullOrEmpty(v))
        {
            var today = DateTime.Now.Date;
            SetCurrentGameDate(today);
            return today;
        }

        if (DateTime.TryParseExact(v, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var d))
            return d;

        var fallback = DateTime.Now.Date;
        SetCurrentGameDate(fallback);
        return fallback;
    }

    public static void SetCurrentGameDate(DateTime date)
    {
        PlayerPrefs.SetString(Prefix(GameDateKey), date.ToString("yyyyMMdd"));
        PlayerPrefs.Save();
    }

    public static DateTime AdvanceGameDate(int days = 1)
    {
        var d = GetCurrentGameDate().AddDays(days);
        SetCurrentGameDate(d);
        return d;
    }

    public static int GetDayNumber()
    {
        return PlayerPrefs.GetInt(Prefix(GameDayKey), 1);
    }

    public static int AdvanceDayNumber(int by = 1)
    {
        int day = GetDayNumber() + by;
        PlayerPrefs.SetInt(Prefix(GameDayKey), day);
        PlayerPrefs.Save();
        return day;
    }

    public static void AddToDailyTotal(float amount)
    {
        if (amount <= 0f) return;

        var key = DailyKey(GetCurrentGameDate());
        float current = PlayerPrefs.GetFloat(key, 0f);
        current += amount;

        PlayerPrefs.SetFloat(key, current);
        PlayerPrefs.Save();
    }

    public static float GetDailyTotal(DateTime date)
    {
        return PlayerPrefs.GetFloat(DailyKey(date), 0f);
    }

    public static float GetTodayTotal()
    {
        return GetDailyTotal(GetCurrentGameDate());
    }

    public static void SetPlayerName(string name)
    {
        PlayerPrefs.SetString(Prefix("PlayerName"), name);
        PlayerPrefs.Save();
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString(Prefix("PlayerName"), "Player");
    }

    public static void SetMoney(int money)
    {
        PlayerPrefs.SetInt(Prefix("Money"), money);
        PlayerPrefs.Save();
    }

    public static int GetMoney()
    {
        return PlayerPrefs.GetInt(Prefix("Money"), 0);
    }

    public static bool SaveExists(int slot)
    {
        return PlayerPrefs.GetInt($"save_{slot}_HasSave", 0) == 1;
    }

    public static void DeleteSave(int slot)
    {
        string prefix = $"save_{slot}_";

        PlayerPrefs.DeleteKey(prefix + "HasSave");
        PlayerPrefs.DeleteKey(prefix + "GameDate");
        PlayerPrefs.DeleteKey(prefix + "GameDayNumber");
        PlayerPrefs.DeleteKey(prefix + "PlayerName");
        PlayerPrefs.DeleteKey(prefix + "Money");
        PlayerPrefs.DeleteKey(prefix + "PlayerCents");

        PlayerPrefs.Save();
    }

    public static void StartNewDay()
    {
        AdvanceGameDate(1);
        AdvanceDayNumber(1);
        PlayerPrefs.Save();
    }

    public static int GetCurrentSlot()
    {
        return currentSlot;
    }
}