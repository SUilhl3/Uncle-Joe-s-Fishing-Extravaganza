using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class hookMovement : MonoBehaviour
{
    public float horzSpeed = 5f;
    public float reelSpeed = 5f;
    public float ambientDropSpeed = -2f;
    public float fishPullForce = 0f;
    public float fishLeftRightForce = 0f;
    public Vector2 moveInput;

    public Transform startingPoint;

    public float dropAmount = 0f;
    public float maxDropAmount = 50f; //maximum distance the hook can drop

    public bool isAtMaxDrop = false;
    public Rigidbody2D rb;
    [SerializeField] private Transform leftBoundaryObj;
    [SerializeField] private Transform rightBoundaryObj;

    [SerializeField] private float leftBoundaryX;
    [SerializeField] private float rightBoundaryX;
    [SerializeField] private float upperBoundaryY;

    [SerializeField] private GameObject castPanel;
    [SerializeField] private bool fishOnHook = false;
    [SerializeField] private Boat_Fish fish = null;

    
    public float lineHealth;
    public float maxLineHealth = 100f;
    public float lineRecoveryRate = 1f;
    public float stamDam = -10f;

    public bool brokenLine = false;

    private enum HookState { dropping, reeling, casting }
    [SerializeField] private HookState currentState = HookState.casting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set horizontal boundaries based on the positions of the boundary objects
        leftBoundaryX = leftBoundaryObj.position.x;
        rightBoundaryX = rightBoundaryObj.position.x;
        InitializeCast();
    }
    void Update()
    {
        if (currentState != HookState.casting)
        {
            if (currentState == HookState.reeling)
            {
                // Move directly toward starting point
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    startingPoint.position,
                    (reelSpeed + fishPullForce) * Time.deltaTime
                );

                // Stop reeling once we reach the start
                if (Vector3.Distance(transform.position, startingPoint.position) < 0.01f)
                {
                    InitializeCast();
                }

                return; // prevent normal movement logic from running
            }
            if (brokenLine)
            {
                // Move directly toward starting point
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    startingPoint.position,
                    reelSpeed * Time.deltaTime
                );

                // Stop reeling once we reach the start
                if (Vector3.Distance(transform.position, startingPoint.position) < 0.01f)
                {
                    InitializeCast();
                }

                return; // prevent normal movement logic from running
            }
            // Calculate movement
            Vector3 movement = new Vector3((moveInput.x + fishLeftRightForce) * horzSpeed, moveInput.y, 0f);

            // Apply movement
            transform.Translate(movement * Time.deltaTime, Space.World);

            // Clamp horizontal position
            float clampedX = Mathf.Clamp(transform.position.x, leftBoundaryX, rightBoundaryX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

            // Existing drop logic stays the same
            if (isAtMaxDrop == false && dropAmount >= maxDropAmount) // check if at max drop amount
            {
                isAtMaxDrop = true; // set flag to true to prevent rerunning this check
                moveInput.y = 0f; // stop dropping if at max drop amount
            }

            if (currentState == HookState.dropping && dropAmount <= maxDropAmount) // if not reeling, increase drop amount unless at max
            {
                dropAmount += -moveInput.y * Time.deltaTime;
            }
            if(fishOnHook)
            {
                moveInput.y = fishPullForce;
            }
        }


    }

    //coroutines
    IEnumerator fishBattle()
    {
        bool fishPulling = fish.getPulling(); //local variables holding values that are used to simulate the fish battle
        float fishStam = fish.getStamina();
        float fishMaxStam = fish.getMaxStam();
        float fishStr = fish.getStrength();
        float fishRecoverRate = fish.getRecoveryRate();
        fishPullForce = fish.getSwimSpeed();
        dropAmount = maxDropAmount; //cap this so it's just the fish pulling and you reeling
        StartCoroutine(leftRight());

        while(fishOnHook)
        {
            fishStam = fish.getStamina();
            fishPulling = fish.getPulling();

            //---------assign if fish is resting or not
            //if the fish runs out of stamina, set to 0, and make the fish take a break
            if(fishStam <= 0 && fishPulling)
            {
                fish.setStam(0f); //set to 0 for safety
                fish.setPulling(false);  //take a break from pulling
                // Debug.Log("Stop pulling, fish should rest");
            }

            //if the fish finishes resting
            if(fishStam >= fishMaxStam && !fishPulling)
            {
                fish.setStam(fishMaxStam); //cap the stamina to max stamina
                fish.setPulling(true); //resume pulling
                // Debug.Log("Start pulling, fish is rested");
            }


            //------------handles logic if the fish is pulling or resting
            if(fishStam > 0 && fishPulling)
            {
                // Debug.Log("Pulling...");
                fishPullForce = -fish.getSwimSpeed();
                if(lineHealth <= 0)
                {
                    lineHealth = 0;
                    fishLeftRightForce = 0f;
                    fish.fishOffHook(3);
                    brokenLine = true;
                    fishOnHook = false;
                    Debug.Log("Line Snapped!");
                }
                //while reeling, if not at 0 linehalth, decrease line health 
                if(currentState == HookState.reeling && lineHealth > 0) //this is under the if of if the fish is pulling so this should only happen when the fish is pulling
                {
                    lineHealth -= fishStr * Time.deltaTime;
                    fish.changeStamina(stamDam * Time.deltaTime);
                }
                //if the line is not at full health and it hasn't snapped yet, recover the linehealth
                else if(lineHealth < maxLineHealth && lineHealth > 0)
                {
                    lineHealth += lineRecoveryRate * Time.deltaTime;
                }
            }

            else if(!fishPulling)
            {
                // Debug.Log("Resting...");
                fishPullForce = 0f;
                //if the fish is resting, should recover line health even if pushing
                if(lineHealth < maxLineHealth && lineHealth > 0) { lineHealth += lineRecoveryRate * Time.deltaTime; }
                fish.changeStamina(fishRecoverRate * Time.deltaTime); //recover the fish's stamina while resting
            }
            yield return null;
        }
        StopCoroutine(fishBattle());
    }

    IEnumerator leftRight()
    {
        while(fishOnHook)
        {
            fishLeftRightForce = Random.Range(-2f, 2f);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    //functions
    public void InitializeCast()
    {
        currentState = HookState.casting;
        transform.position = startingPoint.position;
        dropAmount = 0f; //reset max line length
        fishLeftRightForce = 0f;
        isAtMaxDrop = false;
        lineHealth = maxLineHealth;
        brokenLine = false;
        fishOnHook = false;
        castPanel.SetActive(true);
        moveInput = Vector2.zero; // Reset movement input
    }


    //input functions
    public void OnMove(InputAction.CallbackContext value) //handles horizontal movement
    {
        Vector2 input = value.ReadValue<Vector2>();
        moveInput.x = input.x;
    }
    
    public void OnCast(InputAction.CallbackContext value)
    {
        currentState = HookState.dropping;
        castPanel.SetActive(false);
        moveInput.y = ambientDropSpeed; //start ambient drop
    }

    public void OnReel(InputAction.CallbackContext value)
    {
        if (currentState == HookState.casting)
        {
            return;
        }

        if (value.performed) //if reel button is pressed then reel the hook up
        {
            // Debug.Log("pressing");
            currentState = HookState.reeling;
            moveInput.y = reelSpeed - fishPullForce;
        }
        else if (value.canceled) //if reel button is released then stop reeling
        {
            currentState = HookState.dropping;
            if (dropAmount >= maxDropAmount) //if at max drop amount, stop dropping
            {
                moveInput.y = 0f;
                return;
            }
            moveInput.y = ambientDropSpeed + fishPullForce; //else resume ambient drop
        }

    }

    public bool getFishOnHook() => fishOnHook;


    public void setFishOnHook(bool value) => fishOnHook = value ? true : false;
    public void setFish(Boat_Fish newFish) => fish = newFish;

    public void startFishBattle() => StartCoroutine(fishBattle());
}
