using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Land_Fishing_Game_Manager : MonoBehaviour
{
    public static Land_Fishing_Game_Manager instance;

    [Header("UI Elements")]
    [SerializeField] Slider castDistanceSlider;
    [SerializeField] Button startFishingButton;
    [SerializeField] Button castButton;
    [SerializeField] FishingDailyUI fishingDailyUI;

    [Header("Caught Item Panel UI")]
    [SerializeField] GameObject caughtItemPanel;
    [SerializeField] TextMeshProUGUI caughtItemName;
    [SerializeField] TextMeshProUGUI caughtItemDescription;
    [SerializeField] Image caughtItemImage;
    [SerializeField] Sprite catchLimitImage;

    [Header("Fishing Mini-Game")]
    [SerializeField] RectTransform item;
    [SerializeField] RectTransform playerBar;
    [SerializeField] Slider progressBar;
    [SerializeField] float progressIncreaseSpeed = 10f;
    [SerializeField] float progressDecreaseSpeed = 1.0f;
    [SerializeField] GameObject fishingMiniGame;
    [SerializeField] float chanceToCatchNothing = 0.9f;

    [Header("Fishing Elements")]
    //[SerializeField] GameObject castingLine;
    //[SerializeField] float maxCastDistance = 10f;
    //[SerializeField] float castSpeed = 2f;
    //[SerializeField] float waterPosition = -3f;
    [SerializeField] List<Item> availableItems;
    [SerializeField] List<Item> unlockableItems;

    [Header("Animator")]
    [SerializeField] Animator playerAnimator;

    bool isFishing = false;
   // bool isCasting = false;
    bool isReturning = false;
    bool isFishingGameActive = false;
    float sliderMovementSpeed = 1.0f;
    float castStrength;
    Vector2 targetPosition;
    Vector2 castStartingPosition;
    Vector2 playerBarStart;
    Vector2 itemStart;
    Vector2 originalPlayerBarSize;
    Fish_AI fishAi;
    Item caughtItem;
    List<Item> storedItems;

    private void Awake()
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
        //castStartingPosition = castingLine.transform.position;
        playerBarStart = playerBar.anchoredPosition;
        itemStart = item.anchoredPosition;
        originalPlayerBarSize = playerBar.sizeDelta;
        fishAi = item.GetComponent<Fish_AI>();
        storedItems = new List<Item>();
        fishingDailyUI = FindFirstObjectByType<FishingDailyUI>();
    }

    private void Start()
    {
        if (FishingDailyLimitManager.HasReachedLimit())
        {
            startFishingButton.gameObject.SetActive(false);
        }
    }

    //Starts the cast distance slider moving up and down
    public void StartFishing()
    {
        castDistanceSlider.gameObject.SetActive(true);
        startFishingButton.gameObject.SetActive(false);
        castButton.gameObject.SetActive(true);
        isFishing = true;
    }

    //Casts the line into the water and starts the mini-game
    public void Cast()
    {
        castButton.gameObject.SetActive(false);
        //castStrength = castDistanceSlider.value;
        playerAnimator.SetTrigger("Casting");  

        //sets where to cast the line based on slider value 
        //setup to be fishing towards the right side for now
        //float castDistance = castStrength * maxCastDistance;

        //float targetX = castStartingPosition.x + castDistance;
        //targetPosition = new Vector2(targetX, waterPosition);

        //isCasting = true;
        isFishing = false;
        castDistanceSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFishing)
        {
            Fishing();
        }

        //if (isCasting)
        //{
        //    Casting();
        //}

        if (isFishingGameActive)
        {
            FishingMiniGame();
        }

        if (isReturning)
        {
            Returning();
        }

        if (FishingDailyLimitManager.HasReachedLimit())
        {
            caughtItemPanel.SetActive(true);
            caughtItemName.text = "Daily Catch Limit Reached";
            caughtItemDescription.text = "Come back tomorrow or buy a Chest for +10 daily fish.";
            caughtItemImage.sprite = catchLimitImage;
            return;
        }
    }

    void Fishing()
    {
        castDistanceSlider.value += sliderMovementSpeed * Time.deltaTime;

        if (castDistanceSlider.value >= 1)
        {
            sliderMovementSpeed = -1.0f;
        }
        else if (castDistanceSlider.value <= 0)
        {
            sliderMovementSpeed = 1.0f;
        }
    }

    //casting the line out to the water 
    public void Casting()
    {
        //Vector2 currentPos = castingLine.transform.position;
        //castingLine.transform.position = Vector2.MoveTowards(currentPos, targetPosition, castSpeed * Time.deltaTime);

        //if (Vector2.Distance(currentPos, targetPosition) < 0.01f)
        //{
            //isCasting = false;

            //checks for chance that nothing is on hook 
            float adjustedChanceToCatchNothing = chanceToCatchNothing;

            if (DailyEffectManager.Instance != null &&
                DailyEffectManager.Instance.HasEffect(DailyEffectType.EmptyWaters))
            {
                adjustedChanceToCatchNothing += DailyEffectManager.Instance.GetFloatValue(DailyEffectType.EmptyWaters);
            }

            adjustedChanceToCatchNothing = Mathf.Clamp01(adjustedChanceToCatchNothing);

            float fishOnHook = UnityEngine.Random.value;
            if (fishOnHook < adjustedChanceToCatchNothing)
            {
                DisplayCaughtNothing(true);
                ResetFishingGame();
                return;
            }

            caughtItem = GetRandomItem();
            fishAi.rarity = caughtItem.itemRarity;

            // Reset player bar size before applying daily penalty
            playerBar.sizeDelta = originalPlayerBarSize;

            if (DailyEffectManager.Instance != null &&
                DailyEffectManager.Instance.HasEffect(DailyEffectType.ClumsyHands))
            {
                float penalty = DailyEffectManager.Instance.GetFloatValue(DailyEffectType.ClumsyHands);
                playerBar.sizeDelta = new Vector2(originalPlayerBarSize.x * (1f - penalty), originalPlayerBarSize.y);
            }

            progressBar.gameObject.SetActive(true);
            progressBar.value = progressBar.maxValue / 3;
            fishingMiniGame.SetActive(true);
            isFishingGameActive = true;
            castButton.gameObject.SetActive(false);
            playerAnimator.SetTrigger("Reeling");
        // }
    }

    //function to select the random item from the list using weighted probabilities
    Item GetRandomItem()
    {
        List<Item> currentPool = new List<Item>();
        currentPool.AddRange(availableItems);

        foreach (Item item in unlockableItems)
        {
            if (item == null) continue;

            if (IsLandItemUnlocked(item.itemName))
            {
                currentPool.Add(item);
            }
        }

        float totalWeight = 0f;
        foreach (Item item in currentPool)
        {
            totalWeight += GetAdjustedCatchWeight(item);
        }

        float randomNum = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (Item item in currentPool)
        {
            currentWeight += GetAdjustedCatchWeight(item);
            if (randomNum <= currentWeight)
            {
                return item;
            }
        }

        return currentPool[0];
    }

    float GetAdjustedCatchWeight(Item item)
    {
        float weight = item.probabilityOfCatch;

        // Permanent shop effects: make Can/Gum/Jug less common
        if (item.itemName == "Can" && PlayerPrefs.GetInt("CanPurchased", 0) == 1)
            weight *= 0.5f;

        if (item.itemName == "Gum" && PlayerPrefs.GetInt("GumPurchased", 0) == 1)
            weight *= 0.5f;

        if (item.itemName == "Jug" && PlayerPrefs.GetInt("JugPurchased", 0) == 1)
            weight *= 0.5f;

        bool isJunk = item.itemName == "Can" || item.itemName == "Gum" || item.itemName == "Jug";

        if (DailyEffectManager.Instance != null)
        {
            if (DailyEffectManager.Instance.HasEffect(DailyEffectType.MagnetBait) && isJunk)
                weight *= (1f - DailyEffectManager.Instance.GetFloatValue(DailyEffectType.MagnetBait));

            if (DailyEffectManager.Instance.HasEffect(DailyEffectType.PollutedWater) && isJunk)
                weight *= (1f + DailyEffectManager.Instance.GetFloatValue(DailyEffectType.PollutedWater));

            bool isRare = item.itemRarity == ItemRarity.RARE || item.itemRarity == ItemRarity.LEGENDARY;

            if (isRare)
            {
                if (DailyEffectManager.Instance.HasEffect(DailyEffectType.RareSurge))
                    weight *= (1f + DailyEffectManager.Instance.GetFloatValue(DailyEffectType.RareSurge));

                if (DailyEffectManager.Instance.HasEffect(DailyEffectType.BadLuck))
                    weight *= (1f - DailyEffectManager.Instance.GetFloatValue(DailyEffectType.BadLuck));
            }
        }

        // Permanent shop effects: Eye and Void increase rare catch chance
        bool isPermanentRare = item.itemRarity == ItemRarity.RARE || item.itemRarity == ItemRarity.LEGENDARY;

        if (isPermanentRare)
        {
            if (PlayerPrefs.GetInt("EyePurchased", 0) == 1)
                weight *= 1.3f;

            if (PlayerPrefs.GetInt("VoidPurchased", 0) == 1)
                weight *= 1.6f;
        }

        return Mathf.Max(0.001f, weight);
    }

    bool IsLandItemUnlocked(string itemName)
    {
        switch (itemName)
        {
            case "Egg":
                return PlayerPrefs.GetInt("EggPurchased", 0) == 1;
            case "Lamp":
                return PlayerPrefs.GetInt("LampPurchased", 0) == 1;
            case "Duck":
                return PlayerPrefs.GetInt("DuckPurchased", 0) == 1;
            case "Duck 2":
                return PlayerPrefs.GetInt("Duck2Purchased", 0) == 1;
            case "Toy":
                return PlayerPrefs.GetInt("ToyPurchased", 0) == 1;
            case "Dragon Fish":
                return PlayerPrefs.GetInt("DragonPurchased", 0) == 1;
            case "Dino":
                return PlayerPrefs.GetInt("DinoPurchased", 0) == 1;
            case "Frog":
                return PlayerPrefs.GetInt("FrogPurchased", 0) == 1;
            case "Patrick House":
                return PlayerPrefs.GetInt("PatrickHousePurchased", 0) == 1;
            case "Egg 2":
                return PlayerPrefs.GetInt("Egg2Purchased", 0) == 1;
            default:
                return false;
        }
    }

    //returning the fishing line back to the player
    public void Returning()
    {
        //castingLine.transform.position = Vector2.MoveTowards(
        //    castingLine.transform.position,
        //    castStartingPosition,
        //    castSpeed * Time.deltaTime
        //);

       // if (Vector2.Distance(castingLine.transform.position, castStartingPosition) < 0.01f)
        //{
            caughtItemPanel.SetActive(false);
            isReturning = false;
            if (!FishingDailyLimitManager.HasReachedLimit()) { startFishingButton.gameObject.SetActive(true); }
        //}
    }

    void FishingMiniGame()
    {
        bool overlapping = isOverlapping(item, playerBar);

        UpdateItemMovement();

        float adjustedIncreaseSpeed = progressIncreaseSpeed;

        if (DailyEffectManager.Instance != null &&
            DailyEffectManager.Instance.HasEffect(DailyEffectType.SlowWaters))
        {
            adjustedIncreaseSpeed *= (1f - DailyEffectManager.Instance.GetFloatValue(DailyEffectType.SlowWaters));
        }

        if (overlapping)
        {
            progressBar.value += adjustedIncreaseSpeed * Time.deltaTime;
        }
        else
        {
            progressBar.value -= progressDecreaseSpeed * Time.deltaTime;
        }

        if (progressBar.value >= 100f)
        {
            CheckCatchItem(true);
        }
        else if (progressBar.value <= 0.0f)
        {
            CheckCatchItem(false);
        }
    }

    //calls the random move method in Fish_Ai to randomly move the item
    void UpdateItemMovement()
    {
        fishAi.RandomMove();
    }

    void CheckCatchItem(bool itemCaught)
    {
        if (!itemCaught)
        {
            ResetFishingGame();
            DisplayCaughtNothing(false);
            return;
        }

        if (!FishingDailyLimitManager.TryRegisterCatch())
        {
            caughtItemPanel.SetActive(true);
            caughtItemName.text = "Daily Catch Limit Reached";
            caughtItemDescription.text = "You can't catch any more fish today.";
            ResetFishingGame();
            return;
        }

        fishingDailyUI.Refresh();
        castDistanceSlider.value = 0;

        if (caughtItem.itemName == "Egg")
        {
            Item newFish = GetRandomFishOnly();
            if (newFish != null)
                caughtItem = newFish;
        }

        if (caughtItem.itemName == "Egg 2")
        {
            Item newFish = GetRandomRareFishOnly();
            if (newFish != null)
                caughtItem = newFish;
        }

        // Lucky Pool: reroll once and keep the better-value result
        if (DailyEffectManager.Instance != null &&
            DailyEffectManager.Instance.HasEffect(DailyEffectType.LuckyPool))
        {
            Item rerolled = GetRandomItem();
            if (rerolled != null && rerolled.itemValue > caughtItem.itemValue)
                caughtItem = rerolled;
        }

        DisplayCaughtItem(caughtItem);
        UpdateInventory();

        // Bonus Catch: first catch of the day gives one extra copy
        if (DailyEffectManager.Instance != null &&
            DailyEffectManager.Instance.HasEffect(DailyEffectType.BonusCatch) &&
            !DailyEffectManager.Instance.HasUsedBonusCatchToday())
        {
            CatchInventoryManager.RegisterCatch(
                caughtItem.itemName,
                caughtItem.itemName,
                caughtItem.isFish,
                caughtItem.fishSize,
                Mathf.RoundToInt(caughtItem.itemValue)
            );

            DailyEffectManager.Instance.MarkBonusCatchUsedToday();
        }

        // Full Nets: chance to catch one extra random item
        if (DailyEffectManager.Instance != null &&
            DailyEffectManager.Instance.HasEffect(DailyEffectType.FullNets))
        {
            float extraChance = DailyEffectManager.Instance.GetFloatValue(DailyEffectType.FullNets);

            if (UnityEngine.Random.value < extraChance)
            {
                Item extraItem = GetRandomItem();
                if (extraItem != null)
                {
                    CatchInventoryManager.RegisterCatch(
                        extraItem.itemName,
                        extraItem.itemName,
                        extraItem.isFish,
                        extraItem.fishSize,
                        Mathf.RoundToInt(extraItem.itemValue)
                    );
                }
            }
        }

        ResetFishingGame();
    }

    //resets everything back to the starting place
    void ResetFishingGame()
    {
        playerAnimator.SetTrigger("Returning");
        fishingMiniGame.SetActive(false);
        castButton.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        isFishingGameActive = false;
        playerBar.anchoredPosition = playerBarStart;
        item.anchoredPosition = itemStart;
        playerBar.sizeDelta = originalPlayerBarSize;
        isReturning = true;
    }

    //displays a panel with all the info of the caught item
    void DisplayCaughtItem(Item item)
    {
        caughtItemPanel.SetActive(true);
        caughtItemName.text = item.itemName;
        caughtItemDescription.text = item.itemDescription + "\nWeight: " + item.itemWeight.ToString() + "g";
        if (item.itemImage)
        {
            caughtItemImage.sprite = item.itemImage;
        }
    }

    // displays message for when player failed to catch fish 
    void DisplayCaughtNothing(bool noHook)
    {
        if (noHook)
        {
            caughtItemPanel.SetActive(true);
            caughtItemName.text = "Nothing On The Hook";
            caughtItemDescription.text = "";
            return;
        }
        caughtItemPanel.SetActive(true);
        caughtItemName.text = "It Got Away!";
        caughtItemDescription.text = "";
    }

    //checks if player bar is overlapping with moving fish/item in mini-game
    bool isOverlapping(RectTransform a, RectTransform b)
    {
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];

        a.GetWorldCorners(cornersA);
        b.GetWorldCorners(cornersB);

        Rect rect1 = new Rect(cornersA[0], cornersA[2] - cornersA[0]);
        Rect rect2 = new Rect(cornersB[0], cornersB[2] - cornersB[0]);

        return rect1.Overlaps(rect2);
    }

    void UpdateInventory()
    {
        storedItems.Add(caughtItem);

        CatchInventoryManager.RegisterCatch(
            caughtItem.itemName,
            caughtItem.itemName,
            caughtItem.isFish,
            caughtItem.fishSize,
            Mathf.RoundToInt(caughtItem.itemValue)
        );
    }

    Item GetRandomFishOnly()
    {
        List<Item> fishPool = new List<Item>();

        foreach (Item item in availableItems)
        {
            if (item != null && item.isFish)
                fishPool.Add(item);
        }

        foreach (Item item in unlockableItems)
        {
            if (item != null && item.isFish && IsLandItemUnlocked(item.itemName))
                fishPool.Add(item);
        }

        if (fishPool.Count == 0)
            return null;

        int index = UnityEngine.Random.Range(0, fishPool.Count);
        return fishPool[index];
    }

    Item GetRandomRareFishOnly()
    {
        List<Item> fishPool = new List<Item>();

        foreach (Item item in availableItems)
        {
            if (item != null && item.isFish &&
                (item.itemRarity == ItemRarity.RARE || item.itemRarity == ItemRarity.LEGENDARY))
            {
                fishPool.Add(item);
            }
        }

        foreach (Item item in unlockableItems)
        {
            if (item != null && item.isFish &&
                IsLandItemUnlocked(item.itemName) &&
                (item.itemRarity == ItemRarity.RARE || item.itemRarity == ItemRarity.LEGENDARY))
            {
                fishPool.Add(item);
            }
        }

        if (fishPool.Count == 0)
            return GetRandomFishOnly();

        int index = UnityEngine.Random.Range(0, fishPool.Count);
        return fishPool[index];
    }
}