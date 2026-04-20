using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boat_Manager : MonoBehaviour
{
    public static Boat_Manager instance;

    public int baitAmount = 5;
    public int fishAmount = 0;
    public float boatValue = 0f;

    public List<Boat_Fish_SO> caughtFish = new List<Boat_Fish_SO>();

    [Header("Boat Fish Pools")]
    public List<Boat_Fish_SO> startingFish = new List<Boat_Fish_SO>();
    public List<Boat_Fish_SO> unlockableFish = new List<Boat_Fish_SO>();

    public FishingDailyUI fishingDailyUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void addBait(int amount)
    {
        baitAmount += amount;
    }

    public void setBait(int amount)
    {
        baitAmount = amount;
    }

    public void addFishToBoat(Boat_Fish_SO fish)
    {
        if (FishingDailyLimitManager.HasReachedLimit())
        {
            Debug.Log($"Daily limit reached! ({FishingDailyLimitManager.GetFishCaughtToday()}/{FishingDailyLimitManager.GetDailyCatchLimit()})");
            return;
        }

        if (!FishingDailyLimitManager.TryRegisterCatch())
        {
            Debug.Log($"Daily limit reached! ({FishingDailyLimitManager.GetFishCaughtToday()}/{FishingDailyLimitManager.GetDailyCatchLimit()})");
            return;
        }

        caughtFish.Add(fish);
        fishAmount++;
        boatValue += fish.value;
        fishingDailyUI.Refresh();

        CatchInventoryManager.RegisterCatch(
            fish.fishName,
            fish.fishName,
            true,
            fish.fishSize,
            fish.value
        );

        Debug.Log($"Caught {fish.fishName}. Total today: {FishingDailyLimitManager.GetFishCaughtToday()}/{FishingDailyLimitManager.GetDailyCatchLimit()}");
    }

    public List<Boat_Fish_SO> GetCurrentBoatFishPool()
    {
        List<Boat_Fish_SO> pool = new List<Boat_Fish_SO>();
        pool.AddRange(startingFish);

        foreach (Boat_Fish_SO fish in unlockableFish)
        {
            if (fish == null) continue;

            if (IsBoatFishUnlocked(fish.fishName))
            {
                pool.Add(fish);
            }
        }

        return pool;
    }

    bool IsBoatFishUnlocked(string fishName)
    {
        switch (fishName)
        {
            case "Crab":
                return PlayerPrefs.GetInt("CrabPurchased", 0) == 1;
            case "Elephant Fish":
                return PlayerPrefs.GetInt("ElephantFishPurchased", 0) == 1;
            case "Unknown Fish":
                return PlayerPrefs.GetInt("UnknownFishPurchased", 0) == 1;
            case "Super Sea Cucumber":
                return PlayerPrefs.GetInt("SuperSeaCucumberPurchased", 0) == 1;
            case "Plate Fish":
                return PlayerPrefs.GetInt("PlateFishPurchased", 0) == 1;
            case "Sea Bun":
                return PlayerPrefs.GetInt("SeaBunPurchased", 0) == 1;
            default:
                return false;
        }
    }

    public void clearBoat()
    {
        caughtFish.Clear();
        fishAmount = 0;
        boatValue = 0f;
    }

    public void useBait()
    {
        if (baitAmount > 0)
        {
            baitAmount--;
        }
        else
        {
            Debug.Log("No more bait! Can't fish.");
        }
    }

    public bool hasBait()
    {
        return baitAmount > 0;
    }
}