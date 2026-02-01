using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
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

    int numFishCaught = 0;
    int numJunkCaught = 0;
    bool isFishing = false;
    bool isCasting = false;
    bool isReturning = false;
    float sliderMovementSpeed = 1.0f;
    float castStrength;
    Vector2 targetPosition;
    Vector2 castStartingPosition;

    [Header("Fishing Elements")]
    [SerializeField] GameObject castingLine;
    [SerializeField]float maxCastDistance = 10f;
    [SerializeField] float castSpeed = 2f;
    [SerializeField] float waterPosition = -3f;
    [SerializeField] List<Item> availableItems;

    private void Awake()
    {
        castDistanceSlider = FindAnyObjectByType<Slider>();
        fishCaught.text = numFishCaught.ToString();
        junkCaught.text = numJunkCaught.ToString(); 
        castStartingPosition = castingLine.transform.position;
    }

    public void StartFishing ()
    {
        Debug.Log("Started Fishing");
        startFishingButton.gameObject.SetActive(false);
        castButton.gameObject.SetActive(true);
        isFishing = true;
    }

    public void Cast ()
    {
        Debug.Log("Casting");
        castStrength = castDistanceSlider.value;


        //sets where to cast the line based on slider value 
        //setup to be fishing towards the right side for now
        float castDistance = castStrength * maxCastDistance;

        //will need to do something to change what fish you get based on cast strength later

        float targetX = castStartingPosition.x + castDistance;
        targetPosition = new Vector2(targetX, waterPosition);


        isCasting = true;
        isFishing = false;
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
                FishingMiniGame();
            }
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
                isReturning = false;
                Debug.Log("Line reset complete");
            }
        }
    }

    void FishingMiniGame ()
    {
        Debug.Log("Starting Mini-Game");

        int n = UnityEngine.Random.Range(0, 100);

        Item caughtItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
        if (caughtItem is LandFish)
        {
            Debug.Log("Caught A Fish!");
            castDistanceSlider.value = 0;
            numFishCaught++;
            DisplayCaughtItem(caughtItem);
            UpdateItemsCaught();
        }
        else
        {
            Debug.Log("Caught Some Junk");
            castDistanceSlider.value = 0;
            numJunkCaught++;
            DisplayCaughtItem(caughtItem);
            UpdateItemsCaught();
        }

        startFishingButton.gameObject.SetActive(true);
        castButton.gameObject.SetActive(false);
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
}
