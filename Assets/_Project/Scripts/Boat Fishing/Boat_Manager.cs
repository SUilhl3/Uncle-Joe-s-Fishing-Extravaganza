using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boat_Manager : MonoBehaviour
{
    public static Boat_Manager instance;
    public int baitAmount = 5;
    public int fishAmount = 0;
    public int maxFish = 5;
    public float boatValue = 0f;
    public List<Boat_Fish_SO> caughtFish;

    void Start()
    {
        instance = this;
    }

    public void addBait(int amount) => baitAmount += amount;
    public void setBait(int amount) => baitAmount = amount;

    public void addFishToBoat(Boat_Fish_SO fish)
    {
        if (fishAmount < maxFish)
        {
            caughtFish.Add(fish);
            fishAmount++;
            boatValue += fish.value;
        }
        else
        {
            Debug.Log("Boat is full! Can't add more fish.");
        }
    }

    public void clearBoat()
    {
        caughtFish.Clear();
        fishAmount = 0;
    }

    public void useBait()
    {
        if(baitAmount > 0) { baitAmount--; } 
        else{ Debug.Log("No more bait! Can't fish."); }
    }

    public bool hasBait() => baitAmount > 0;
}
