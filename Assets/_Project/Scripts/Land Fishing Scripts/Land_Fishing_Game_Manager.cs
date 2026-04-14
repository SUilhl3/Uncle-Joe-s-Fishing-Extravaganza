using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Land_Fishing_Game_Manager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Slider castDistanceSlider;
    [SerializeField] Button startFishingButton;
    [SerializeField] Button castButton;
    [SerializeField] TextMeshProUGUI baitText;

    [Header("Caught Item Panel UI")]
    [SerializeField] GameObject caughtItemPanel;
    [SerializeField] TextMeshProUGUI caughtItemName;
    [SerializeField] TextMeshProUGUI caughtItemDescription;
    [SerializeField] Image caughtItemImage;

    [Header("Fishing Mini-Game")]
    [SerializeField] RectTransform item;
    [SerializeField] RectTransform playerBar;
    [SerializeField] Slider progressBar;
    [SerializeField] float progressIncreaseSpeed = 10f;
    [SerializeField] float progressDecreaseSpeed = 1.0f;
    [SerializeField] GameObject fishingMiniGame;
    [SerializeField] float chanceToCatchNothing = 0.9f;
    [SerializeField] int numBait =5;

    [Header("Fishing Elements")]
    [SerializeField] GameObject castingLine;
    [SerializeField] float maxCastDistance = 10f;
    [SerializeField] float castSpeed = 2f;
    [SerializeField] float waterPosition = -3f;
    [SerializeField] List<Item> availableItems;



    bool isFishing = false;
    bool isCasting = false;
    bool isReturning = false;
    bool isFishingGameActive = false;
    float sliderMovementSpeed = 1.0f;
    float castStrength;
    Vector2 targetPosition;
    Vector2 castStartingPosition;
    Vector2 playerBarStart;
    Vector2 itemStart;
    Fish_AI fishAi;
    Item caughtItem;
    List<Item> storedItems;

    private void Awake()
    {
        castStartingPosition = castingLine.transform.position;
        playerBarStart = playerBar.anchoredPosition;
        itemStart = item.anchoredPosition;
        fishAi = item.GetComponent<Fish_AI>();
        storedItems = new List<Item>();
        baitText.text = "Bait: " + numBait;
    }

    //Starts the cast distance slider moving up and down
    public void StartFishing()
    {
        if (FishingDailyLimitManager.HasReachedLimit())
        {
            caughtItemPanel.SetActive(true);
            caughtItemName.text = "Daily Catch Limit Reached";
            caughtItemDescription.text = "Come back tomorrow or buy a Chest for +10 daily fish.";
            return;
        }

        castDistanceSlider.gameObject.SetActive(true);
        startFishingButton.gameObject.SetActive(false);
        castButton.gameObject.SetActive(true);
        isFishing = true;
    }

    //Casts the line into the water and starts the mini-game
    public void Cast ()
    {
        castStrength = castDistanceSlider.value;


        //sets where to cast the line based on slider value 
        //setup to be fishing towards the right side for now
        float castDistance = castStrength * maxCastDistance;

        //will need to do something to change what fish you get based on cast strength later

        float targetX = castStartingPosition.x + castDistance;
        targetPosition = new Vector2(targetX, waterPosition);


        isCasting = true;
        isFishing = false;
        castDistanceSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Moves cast strength slider up and down
        if (isFishing)
        {
            Fishing();
        }

        //moves casting line into the water 
        if (isCasting)
        {
            Casting();
        }

        if (isFishingGameActive)
        {
            FishingMiniGame();
        }

        //returns the casting line back to the starting position after catching or not catching a fish
        if (isReturning)
        {
            Returning();
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
    void Casting()
    {
        Vector2 currentPos = castingLine.transform.position;
        castingLine.transform.position = Vector2.MoveTowards(currentPos, targetPosition, castSpeed * Time.deltaTime);

        if (Vector2.Distance(currentPos, targetPosition) < 0.01f)
        {
            isCasting = false;

            //checks for chance that nothing is on hook 
            float fishOnHook = UnityEngine.Random.value;
            if (fishOnHook <chanceToCatchNothing)
            {
                DisplayCaughtNothing(true);
                ResetFishingGame();
                return;
            }

            //changes type of fish/items can catch based on castStrength
            //does nothing for now 
            if (castStrength >= 0.01 && castStrength <= 0.4)
            {
                Debug.Log("Low Cast Strength");
            }
            else if (castStrength > 0.4 && castStrength <= 0.7)
            {
                Debug.Log("Med Cast Strength");
            }
            else
            {
                Debug.Log("High Cast Strength");
            }

            //sets difficulty for fishing mini game based on enum value of fish/junk
            //caughtItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
            caughtItem = GetRandomItem();
            fishAi.rarity = caughtItem.itemRarity;

            progressBar.gameObject.SetActive(true);
            progressBar.value = progressBar.maxValue / 3;
            fishingMiniGame.SetActive(true);
            isFishingGameActive = true;
            castButton.gameObject.SetActive(false);
        }
    }

    //function to select the random item from the list using weighted probabilities
    Item GetRandomItem()
    {
        float totalWeight = 0f;
        foreach(Item item in availableItems)
        {
            totalWeight += item.probabilityOfCatch;
        }

        float randomNum = UnityEngine.Random.Range(0f, totalWeight);
        float currentWright = 0f;

        foreach (Item item in availableItems)
        {
            currentWright += item.probabilityOfCatch;
            if (randomNum <= currentWright)
            {
                return item;
            }
        }

        //default item returned if above somehow errors
        return availableItems[0];
    }


    //returning the fishing line back to the player
    void Returning()
    {
        castingLine.transform.position = Vector2.MoveTowards(
            castingLine.transform.position,
            castStartingPosition,
            castSpeed * Time.deltaTime
        );

        if (Vector2.Distance(castingLine.transform.position, castStartingPosition) < 0.01f)
        {
            caughtItemPanel.SetActive(false);
            isReturning = false;
            if(numBait != 0) { startFishingButton.gameObject.SetActive(true); }
        }
    }

    void FishingMiniGame ()
    {
        bool overlapping = isOverlapping(item, playerBar);

        UpdateItemMovement();

        if (overlapping)
        {
            progressBar.value += progressIncreaseSpeed * Time.deltaTime;
        } else
        {
            progressBar.value -= progressDecreaseSpeed * Time.deltaTime;
        }

        if (progressBar.value >= 100f)
        {
            CheckCatchItem(true);
        } else if (progressBar.value <= 0.0f)
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
        numBait--;
        baitText.text = "Bait: " + numBait;

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

        castDistanceSlider.value = 0;
        DisplayCaughtItem(caughtItem);
        UpdateInventory();

        ResetFishingGame();
    }

    //resets everything back to the starting place
    void ResetFishingGame()
    {
        fishingMiniGame.SetActive(false);
        castButton.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        isFishingGameActive = false;
        playerBar.anchoredPosition = playerBarStart;
        item.anchoredPosition = itemStart;
        isReturning = true;
        
    }

    //displays a panel with all the info of the caught item
    //panel is permanently there for now, will make into a popup later
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
    bool isOverlapping (RectTransform a, RectTransform b)
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
        //stores fish in inventory (TO DO)
        storedItems.Add(caughtItem);
    }
}
