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

        int requestCount = Mathf.Min(5, available.Count);

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
        return Mathf.CeilToInt(entry.baseValue * 1.2f);
    }
}