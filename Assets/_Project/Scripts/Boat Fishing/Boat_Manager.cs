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

        Debug.Log($"Caught {fish.fishName}. Total today: {FishingDailyLimitManager.GetFishCaughtToday()}/{FishingDailyLimitManager.GetDailyCatchLimit()}");
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