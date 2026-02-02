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

    [Header("Fishing Elements")]
    [SerializeField] GameObject castingLine;
    [SerializeField]float maxCastDistance = 10f;
    [SerializeField] float castSpeed = 2f;
    [SerializeField] float waterPosition = -3f;
    [SerializeField] List<Item> availableItems;

    private void Awake()
    {
        fishCaught.text = numFishCaught.ToString();
        junkCaught.text = numJunkCaught.ToString(); 
        castStartingPosition = castingLine.transform.position;
        playerBarStart = playerBar.anchoredPosition;
    }

    public void StartFishing ()
    {
        castDistanceSlider.gameObject.SetActive(true);
        startFishingButton.gameObject.SetActive(false);
        castButton.gameObject.SetActive(true);
        isFishing = true;
    }

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

    //Updates num items caught mostly for testing can remove later when we have an actual UI
    void UpdateItemsCaught()
    {
        fishCaught.text = numFishCaught.ToString();
        junkCaught.text = numJunkCaught.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Moves cast strength slider up and down
        if (isFishing)
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

        //moves casting line into the water 
        if (isCasting)
        {
            Vector2 currentPos = castingLine.transform.position;
            castingLine.transform.position = Vector2.MoveTowards(currentPos, targetPosition, castSpeed * Time.deltaTime);

            if (Vector2.Distance(currentPos, targetPosition) < 0.01f)
            {
                isCasting = false;
                progressBar.gameObject.SetActive(true);
                progressBar.value = 0.5f;
                fishingMiniGame.SetActive(true);
                isFishingGameActive = true;
            }
        }

        if (isFishingGameActive)
        {
            FishingMiniGame();
        }

        //returns the casting line back to the starting position after catching or not catching a fish
        if (isReturning)
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
    }

    void FishingMiniGame ()
    {
        bool overlapping = isOverlapping(item, playerBar);

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

    void CheckCatchItem(bool itemCaught)
    {

        if (!itemCaught)
        {
            startFishingButton.gameObject.SetActive(true);
            castButton.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
            isFishingGameActive = false;
            playerBar.anchoredPosition = playerBarStart;
            fishingMiniGame.SetActive(false);
            isReturning = true;
            DisplayCaughtNothing();
            return;
        }

        int n = UnityEngine.Random.Range(0, 100);

        Item caughtItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
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

        startFishingButton.gameObject.SetActive(true);
        castButton.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        isFishingGameActive = false;
        playerBar.anchoredPosition = playerBarStart;
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

    void DisplayCaughtNothing()
    {
        caughtItemPanel.SetActive(true);
        caughtItemName.text = "It Got Away!";
        caughtItemDescription.text = "";

    }

    //checks if player bar is overlapping with moving fish/item in mini-game
    //not really working right now
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
}
