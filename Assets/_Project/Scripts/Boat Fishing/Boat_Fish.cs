using UnityEngine;
using System.Collections;

public class Boat_Fish : MonoBehaviour
{
    //temporary starting script, going to be overhauled later on
    //this is just going to do the basic fish movement and behavior for now

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
    [SerializeField] private hookMovement hookScript;

    private enum FishState { Wandering, ChasingHook, OnHook, returningHome, caught }
    [SerializeField] private FishState currentState = FishState.Wandering;

    //fish rotations
    [SerializeField] private float xRotation = 0f;

    private void Start()
    {
        originalScale = transform.localScale;
        // Set up the fish's mouth collider
        fish_collider = GetComponent<CircleCollider2D>();
        fish_collider.offset = mouthOffset;

        //start the fish's random movement
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        StartCoroutine(randomizeDirection());

        //creating the home area collider
        GameObject homeAreaObject = new GameObject("HomeArea_" + name + "_" + GetInstanceID());
        homeAreaObject.transform.position = transform.position; // Position the home area at the fish's starting position
        home_area = homeAreaObject.AddComponent<CircleCollider2D>();
        home_area.radius = 5f; // Set the radius of the home area
        home_area.isTrigger = true;

        hookScript = FindFirstObjectByType<hookMovement>();
    }

    private void Update()
    {
        if(currentState == FishState.caught)
        {
            return;
        }
        else if (currentState == FishState.OnHook)
        {
            Debug.Log("Fish is on the hook!");
        }
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
        while (currentState != FishState.ChasingHook && currentState != FishState.OnHook)
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
            currentState = FishState.caught;
            transform.SetParent(null); 
            transform.position = boat.position; 
            swimSpeed = 0f;
            handleRotate();
            hookScript.InitializeCast(); //reset the hook for the next cast
            hookScript.setFishOnHook(false);
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
            swimSpeed = 0f;
            currentState = FishState.OnHook;
            hookScript.setFishOnHook(true);   
            }
        }
    }

    public void handleRotate()
    {
        if(currentState == FishState.caught)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            return;
        }
        // Rotate to face movement direction
            xRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            if (xRotation > 90 || xRotation < -90) { transform.localScale = new Vector3(transform.localScale.x, -1f, transform.localScale.z); }
            else { transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z); }

            transform.rotation = Quaternion.Euler(0f, 0f, xRotation);
    }

    public bool getChasingHook() => currentState == FishState.ChasingHook;



    public void setChasingHook(bool value)
    {
        if(currentState == FishState.OnHook || currentState == FishState.caught) { return; } //if the fish is on the hook or caught, don't allow it to chase the hook
        else { currentState = value ? FishState.ChasingHook : FishState.returningHome; }
    }
    public void setHookPosition(Vector3 position) => hookPosition = position;

}
