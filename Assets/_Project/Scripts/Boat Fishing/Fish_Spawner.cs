using UnityEngine;
using System.Collections.Generic;

public class Fish_Spawner : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numberOfFishToSpawn = 5;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [SerializeField] List<GameObject> spawnedFish = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < numberOfFishToSpawn; i++)
        {
            SpawnFish();
        }
    }

    void SpawnFish()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );
        GameObject newFish = Instantiate(fishPrefab, spawnPos, Quaternion.identity, transform);
        spawnedFish.Add(newFish);
    }

    // Update is called once per frame
    void Update()
    {
        //respawn fish once you catch one so there are always fish in the water
        while (spawnedFish.Count < numberOfFishToSpawn)
        {
            SpawnFish();
        }
    }

    public void RemoveFish(Boat_Fish fish)
    {
        //remove a fish from the list if it's been caught
        spawnedFish.Remove(fish.gameObject);
    }
}
