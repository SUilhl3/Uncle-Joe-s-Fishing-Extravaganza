using System;
using UnityEngine;

public static class SaveManager
{
    const string GameDateKey = "GameDate";
    const string GameDayKey = "GameDayNumber";

    static string DailyKey(DateTime date) => $"TotalEarned_{date:yyyyMMdd}";

    public static DateTime GetCurrentGameDate()
    {
        string v = PlayerPrefs.GetString(GameDateKey, "");
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
        PlayerPrefs.SetString(GameDateKey, date.ToString("yyyyMMdd"));
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
        return PlayerPrefs.GetInt(GameDayKey, 1);
    }

    public static int AdvanceDayNumber(int by = 1)
    {
        int day = GetDayNumber() + by;
        PlayerPrefs.SetInt(GameDayKey, day);
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
        var key = DailyKey(date);
        return PlayerPrefs.GetFloat(key, 0f);
    }

    public static float GetTodayTotal()
    {
        return GetDailyTotal(GetCurrentGameDate());
    }
}
