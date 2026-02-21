using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Land_Fishing_Game_Manager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Slider castDistanceSlider;
    [SerializeField] Button startFishingButton;
    [SerializeField] Button castButton;
    [SerializeField] TextMeshProUGUI fishCaught;
    [SerializeField] TextMeshProUGUI junkCaught;

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

    [Header("Fishing Elements")]
    [SerializeField] GameObject castingLine;
    [SerializeField] float maxCastDistance = 10f;
    [SerializeField] float castSpeed = 2f;
    [SerializeField] float waterPosition = -3f;
    [SerializeField] List<Item> availableItems;



    int numFishCaught = 0;
    int numJunkCaught = 0;
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

    private void Awake()
    {
        fishCaught.text = numFishCaught.ToString();
        junkCaught.text = numJunkCaught.ToString(); 
        castStartingPosition = castingLine.transform.position;
        playerBarStart = playerBar.anchoredPosition;
        itemStart = item.anchoredPosition;
        fishAi = item.GetComponent<Fish_AI>();
    }

    //Starts the cast distance slider moving up and down
    public void StartFishing ()
    {
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

            //change
            caughtItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
            fishAi.moveSpeed = caughtItem.moveSpeed;
            fishAi.directionChangeInterval = caughtItem.directionChangeInterval;

            progressBar.gameObject.SetActive(true);
            progressBar.value = progressBar.maxValue / 3;
            fishingMiniGame.SetActive(true);
            isFishingGameActive = true;
            castButton.gameObject.SetActive(false);
        }
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

        if (!itemCaught)
        {
            ResetFishingGame();
            DisplayCaughtNothing();
            return;
        }

        
        if (caughtItem is LandFish)
        {
            castDistanceSlider.value = 0;
            numFishCaught++;
            DisplayCaughtItem(caughtItem);
            UpdateItemsCaught();
        }
        else
        {
            castDistanceSlider.value = 0;
            numJunkCaught++;
            DisplayCaughtItem(caughtItem);
            UpdateItemsCaught();
        }

        ResetFishingGame();
    }

    //resets everything back to the starting place
    void ResetFishingGame()
    {
        startFishingButton.gameObject.SetActive(true);
        castButton.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        isFishingGameActive = false;
        playerBar.anchoredPosition = playerBarStart;
        item.anchoredPosition = itemStart;
        fishingMiniGame.SetActive(false);
        isReturning = true;
    }

    //displays a panel with all the info of the caught item
    //panel is permanently there for now, will make into a popup later
    void DisplayCaughtItem(Item item)
    {
        caughtItemPanel.SetActive(true);
        caughtItemName.text = item.itemName;
        caughtItemDescription.text = item.itemDescription;
        if (item.itemImage)
        {
            caughtItemImage.sprite = item.itemImage;
        }
    }

    // displays message for when player failed to catch fish 
    void DisplayCaughtNothing()
    {
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

    //Updates num items caught mostly for testing can remove later when we have an actual UI
    void UpdateItemsCaught()
    {
        fishCaught.text = numFishCaught.ToString();
        junkCaught.text = numJunkCaught.ToString();
    }
}
