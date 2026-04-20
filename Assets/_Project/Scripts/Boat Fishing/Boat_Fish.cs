using UnityEngine;
using System.Collections;

public class Boat_Fish : MonoBehaviour
{
    //temporary starting script, going to be overhauled later on
    //this is just going to do the basic fish movement and behavior for now

    [SerializeField] private Boat_Fish_SO fishSO;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] private float swimSpeed = 2f;
    [SerializeField] private Vector2 wanderDirection;
    [SerializeField] private Vector2 prevWanderDirection;
    [SerializeField] private GameObject fish;
    [SerializeField] private Vector3 hookPosition;
    [SerializeField] private Vector2 mouthOffset = new Vector2(-0.5f, 0f); // Adjust this based on the fish's sprite and size
    [SerializeField] private CircleCollider2D fish_collider; //the fish's mouth collider
    [SerializeField] private CircleCollider2D home_area; //going to be manually set for now
    [SerializeField] private Vector3 movement;
    [SerializeField] private Vector2 moveDirection = new Vector2(2f, 2f);
    [SerializeField] private Transform boat;
    private Vector3 originalScale;

    [Header("Fish Combat stats")]
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina = 50f; //really high to have a longer fish battle with periods of free reeling
    [SerializeField] private float strength = 5f;
    [SerializeField] private float recoverRate = 15f; //pretty high because the fish is only supposed to give a few second window of free reeling
    [SerializeField] private bool pulling = false;


    [SerializeField] private hookMovement hookScript;

    private enum FishState { Wandering, ChasingHook, OnHook, returningHome, caught }
    [SerializeField] private FishState currentState = FishState.Wandering;

    //fish rotations
    [SerializeField] private float xRotation = 0f;
    [SerializeField] private float yRotation = 0f;
    [SerializeField] private float zRotation = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boat = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        fishSO = Boat_Manager.instance.GetRandomBoatFishOnly();
        //fishSO = Boat_Manager.instance.startingFish[Random.Range(0, Boat_Manager.instance.startingFish.Count)];
        spriteRenderer.sprite = fishSO.fishSprite;
        originalScale = transform.localScale;
        // Set up the fish's mouth collider
        fish_collider = GetComponent<CircleCollider2D>();
        fish_collider.offset = mouthOffset;

        //start the fish's random movement
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        StartCoroutine(randomizeDirection()); //might try to find a new way to do this since multiple coroutines might be a bit taxxing

        //creating the home area collider
        GameObject homeAreaObject = new GameObject("HomeArea_" + name + "_" + GetInstanceID());
        homeAreaObject.transform.position = transform.position; // Position the home area at the fish's starting position
        home_area = homeAreaObject.AddComponent<CircleCollider2D>();
        home_area.radius = 5f; // Set the radius of the home area
        home_area.isTrigger = true;

        hookScript = FindFirstObjectByType<hookMovement>();

        //fish combat init
        stamina = maxStamina;
    }

    private void Update()
    {
        if(currentState == FishState.caught) { return; }
        else if (currentState == FishState.OnHook) { }

        else
        {
            if (currentState != FishState.ChasingHook)
            {
                if (!home_area.OverlapPoint(transform.position)) // If the fish is outside the home area, move it back towards the center
                {
                    currentState = FishState.returningHome;
                    prevWanderDirection = wanderDirection;
                    Vector3 homeCenter = home_area.transform.position;
                    moveDirection = (homeCenter - transform.position).normalized;
                    movement = moveDirection * swimSpeed * Time.deltaTime;
                }
                else if (prevWanderDirection != wanderDirection)// If the fish is inside the home area, move it in the current direction
                {
                    currentState = FishState.Wandering;
                    prevWanderDirection = new Vector2(2f, 2f);
                    moveDirection = wanderDirection;
                    movement = moveDirection * swimSpeed * Time.deltaTime;
                }
            }
            if (currentState == FishState.ChasingHook) //if chasing the hook, move towards the hook's position
            {
                moveDirection = (hookPosition - transform.position).normalized;
                movement = moveDirection * swimSpeed * Time.deltaTime;
                if(hookScript.getFishOnHook())
                {
                    currentState = FishState.returningHome;
                }
            }
            transform.Translate(movement, Space.World);

            handleRotate();
        }


    }

    IEnumerator randomizeDirection()
    {
        while (/*currentState != FishState.ChasingHook && currentState != FishState.OnHook*/true)
        {
            // Randomly change direction every 2-5 seconds
            wanderDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            // Debug.Log("Fish changed direction to: " + moveDirection);
            yield return new WaitForSeconds(Random.Range(2f, 3f));
        }
    }

    // Handle collision with the hook
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("catchArea"))
        {
            fishOffHook(4);
            transform.position = boat.position; 
            Boat_Manager.instance.addFishToBoat(fishSO);
            Fish_Spawner fishSpawner = FindFirstObjectByType<Fish_Spawner>();
            fishSpawner.RemoveFish(this);
            hookScript.InitializeCast(); //reset the hook for the next cast
        }
        else if (collision.gameObject.CompareTag("Hook"))
        {
            if(hookScript.getFishOnHook())
            {
                return;
            }
            else
            {
                transform.SetParent(collision.transform); //make the fish a child of the hook so it moves with it
                transform.localScale = originalScale;
                currentState = FishState.OnHook;
                pulling = true;
                hookScript.setFishOnHook(true);   
                hookScript.setFish(this);
                hookScript.startFishBattle();
            }
        }
    }

    public void fishOffHook(int n)
    {
        currentState = (FishState)n;
        transform.SetParent(null);
        handleRotate();
        hookScript.setFishOnHook(false);
    }

    public void handleRotate()
    {
        if(currentState == FishState.caught)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -45f);
            return;
        }
        // Rotate to face movement direction
        zRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 45f; 
        float finalZRotation = zRotation;

        if(zRotation > 45 || zRotation < -135)
        {
            // yRotation = 180f;
            transform.localScale = new Vector3(transform.localScale.x, -1f * Mathf.Abs(transform.localScale.y), transform.localScale.z);
            finalZRotation += 100f;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
            transform.rotation = Quaternion.Euler(yRotation, 0f, finalZRotation);
    }

    public bool getChasingHook() => currentState == FishState.ChasingHook;
    public float getStamina() => stamina;
    public bool getPulling() => pulling;
    public float getMaxStam() => maxStamina;
    public float getStrength() => strength;
    public float getRecoveryRate() => recoverRate;
    public float getSwimSpeed() => swimSpeed;



    public void setChasingHook(bool value)
    {
        if(currentState == FishState.OnHook || currentState == FishState.caught) { return; } //if the fish is on the hook or caught, don't allow it to chase the hook
        else { currentState = value ? FishState.ChasingHook : FishState.returningHome; }
    }
    public void setHookPosition(Vector3 position) => hookPosition = position;

    public void changeStamina(float amount) => stamina += amount;
    public void setStam(float value) => stamina = value;
    public void setPulling(bool value) => pulling = value;

}
