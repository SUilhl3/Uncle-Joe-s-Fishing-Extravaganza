using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CatchInventoryEntry
{
    public string id;
    public string displayName;
    public bool isFish;
    public FishSize fishSize;
    public int value;
    public int count;
}

[Serializable]
public class CatchInventorySaveData
{
    public List<CatchInventoryEntry> entries = new List<CatchInventoryEntry>();
}

public static class CatchInventoryManager
{
    private const string InventoryKey = "CatchInventory";

    private static CatchInventorySaveData cachedData;

    private static CatchInventorySaveData Load()
    {
        if (cachedData != null)
            return cachedData;

        string json = PlayerPrefs.GetString(InventoryKey, "");
        if (string.IsNullOrEmpty(json))
        {
            cachedData = new CatchInventorySaveData();
        }
        else
        {
            cachedData = JsonUtility.FromJson<CatchInventorySaveData>(json);
            if (cachedData == null)
                cachedData = new CatchInventorySaveData();
        }

        return cachedData;
    }

    private static void Save()
    {
        string json = JsonUtility.ToJson(cachedData);
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();
    }

    public static List<CatchInventoryEntry> GetAllEntries()
    {
        return Load().entries;
    }

    public static CatchInventoryEntry GetEntry(string id)
    {
        var data = Load();
        return data.entries.Find(e => e.id == id);
    }

    public static int GetCount(string id)
    {
        var entry = GetEntry(id);
        return entry != null ? entry.count : 0;
    }

    public static void RegisterCatch(string id, string displayName, bool isFish, FishSize fishSize, int value)
    {
        var data = Load();
        var entry = data.entries.Find(e => e.id == id);

        if (entry == null)
        {
            entry = new CatchInventoryEntry
            {
                id = id,
                displayName = displayName,
                isFish = isFish,
                fishSize = fishSize,
                value = value,
                count = 0
            };
            data.entries.Add(entry);
        }

        entry.count++;
        Save();
    }

    public static bool RemoveItem(string id, int amount = 1)
    {
        var entry = GetEntry(id);
        if (entry == null || entry.count < amount)
            return false;

        entry.count -= amount;

        if (entry.count <= 0)
        {
            Load().entries.Remove(entry);
        }

        Save();
        return true;
    }

    public static bool HasAnyInventory()
    {
        foreach (var entry in GetAllEntries())
        {
            if (entry.count > 0)
                return true;
        }
        return false;
    }

    public static bool HasAnyFishForRestaurant()
    {
        foreach (var entry in GetAllEntries())
        {
            if (entry.isFish && entry.count > 0)
                return true;
        }
        return false;
    }

    public static int GetFishCountBySize(FishSize size)
    {
        int total = 0;

        foreach (var entry in GetAllEntries())
        {
            if (entry.isFish && entry.fishSize == size)
                total += entry.count;
        }

        return total;
    }

    public static bool ConsumeFish(FishSize size, int amount = 1)
    {
        var entries = GetAllEntries();

        foreach (var entry in entries)
        {
            if (entry.isFish && entry.fishSize == size && entry.count >= amount)
            {
                entry.count -= amount;

                if (entry.count <= 0)
                    entries.Remove(entry);

                Save();
                return true;
            }
        }

        return false;
    }

    public static CatchInventorySaveData Export() 
    {
        return Load(); 
    }

    public static void Import(CatchInventorySaveData data) 
    {
        cachedData = data ?? new CatchInventorySaveData(); 
        Save(); 
    }

    public static bool RemoveRandomItem()
    {
        var entries = GetAllEntries();

        List<CatchInventoryEntry> validEntries = new List<CatchInventoryEntry>();

        foreach (var entry in entries)
        {
            if (entry != null && entry.count > 0)
                validEntries.Add(entry);
        }

        if (validEntries.Count == 0)
            return false;

        int randomIndex = UnityEngine.Random.Range(0, validEntries.Count);
        CatchInventoryEntry chosen = validEntries[randomIndex];

        chosen.count--;

        if (chosen.count <= 0)
            entries.Remove(chosen);

        Save();
        return true;
    }
}