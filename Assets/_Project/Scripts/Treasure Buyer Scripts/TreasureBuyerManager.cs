using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreasureOfferSaveData
{
    public List<string> offeredIds = new List<string>();
}

public class TreasureBuyerManager : MonoBehaviour
{
    [SerializeField] private List<TreasureBuyerCatalogEntry> catalog = new List<TreasureBuyerCatalogEntry>();
    [SerializeField] private Transform contentRoot;
    [SerializeField] private TreasureBuyerRowUI rowPrefab;

    private List<TreasureBuyerCatalogEntry> todaysOffers = new List<TreasureBuyerCatalogEntry>();

    private string OfferKey =>
        "TreasureOffers_" + SaveManager.GetCurrentGameDate().ToString("yyyyMMdd");

    void Start()
    {
        LoadOrGenerateOffers();
        BuildUI();
    }

    void LoadOrGenerateOffers()
    {
        todaysOffers.Clear();

        string json = PlayerPrefs.GetString(OfferKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            TreasureOfferSaveData data = JsonUtility.FromJson<TreasureOfferSaveData>(json);
            if (data != null)
            {
                foreach (string id in data.offeredIds)
                {
                    TreasureBuyerCatalogEntry match = catalog.Find(c => c.id == id);
                    if (match != null)
                        todaysOffers.Add(match);
                }
            }
        }

        if (todaysOffers.Count > 0)
            return;

        List<TreasureBuyerCatalogEntry> available = new List<TreasureBuyerCatalogEntry>();

        foreach (var entry in catalog)
        {
            if (string.IsNullOrEmpty(entry.unlockKey) || PlayerPrefs.GetInt(entry.unlockKey, 0) == 1)
            {
                available.Add(entry);
            }
        }

        int requestCount = 5;

        // Shop upgrades that increase daily treasure buyer requests
        if (PlayerPrefs.GetInt("PaintingPurchased", 0) == 1) requestCount += 1;
        if (PlayerPrefs.GetInt("StackOfBooksPurchased", 0) == 1) requestCount += 1;

        requestCount = Mathf.Min(requestCount, available.Count);

        for (int i = 0; i < requestCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, available.Count);
            todaysOffers.Add(available[randomIndex]);
            available.RemoveAt(randomIndex);
        }

        TreasureOfferSaveData saveData = new TreasureOfferSaveData();
        foreach (var offer in todaysOffers)
            saveData.offeredIds.Add(offer.id);

        PlayerPrefs.SetString(OfferKey, JsonUtility.ToJson(saveData));
        PlayerPrefs.Save();
    }

    void BuildUI()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        foreach (var offer in todaysOffers)
        {
            TreasureBuyerRowUI row = Instantiate(rowPrefab, contentRoot);
            row.Setup(offer, GetOfferPrice(offer));
        }
    }

    int GetOfferPrice(TreasureBuyerCatalogEntry entry)
    {
        float multiplier = 5f;

        // Daily card effects
        if (DailyEffectManager.Instance != null)
        {
            if (DailyEffectManager.Instance.HasEffect(DailyEffectType.HighDemand))
                multiplier += DailyEffectManager.Instance.GetFloatValue(DailyEffectType.HighDemand);

            if (DailyEffectManager.Instance.HasEffect(DailyEffectType.MarketCrash))
                multiplier -= DailyEffectManager.Instance.GetFloatValue(DailyEffectType.MarketCrash);
        }

        // Shop upgrades
        if (PlayerPrefs.GetInt("ChickenNuggetPurchased", 0) == 1)
            multiplier += 0.20f;

        if (PlayerPrefs.GetInt("MirrorPurchased", 0) == 1)
            multiplier += 0.10f;

        if (PlayerPrefs.GetInt("AlsoMirrorPurchased", 0) == 1)
            multiplier += 0.10f;

        return Mathf.CeilToInt(entry.baseValue * multiplier);
    }
}