using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class hookMovement : MonoBehaviour
{
    public float horzSpeed = 5f;
    public float reelSpeed = 5f;
    public float ambientDropSpeed = -2f;
    public Vector2 moveInput;

    public Transform startingPoint;

    public float dropAmount = 0f;
    public float maxDropAmount = 50f; //maximum distance the hook can drop

    public bool isReeling = false;
    public bool isAtMaxDrop = false;
    public Rigidbody2D rb;
    [SerializeField] private Transform leftBoundaryObj;
    [SerializeField] private Transform rightBoundaryObj;

    [SerializeField] private float leftBoundaryX;
    [SerializeField] private float rightBoundaryX;

    [SerializeField] private GameObject castPanel;
    [SerializeField] private bool fishOnHook = false;

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
            // Calculate movement
            Vector3 movement = new Vector3(moveInput.x * horzSpeed, moveInput.y, 0f);

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
        }


    }


    //input functions
    public void OnMove(InputAction.CallbackContext value) //handles horizontal movement
    {
        Vector2 input = value.ReadValue<Vector2>();
        moveInput.x = input.x;
    }

    public void InitializeCast()
    {
        currentState = HookState.casting;
        transform.position = startingPoint.position; 

        castPanel.SetActive(true);
        moveInput = Vector2.zero; // Reset movement input
    }
    
    public void OnCast(InputAction.CallbackContext value)
    {
        currentState = HookState.dropping;
            castPanel.SetActive(false);
            moveInput.y = ambientDropSpeed; //start ambient drop
    }

    public void OnReel(InputAction.CallbackContext value)
    {
        if(currentState == HookState.casting)
        {
            return;
        }

        if (value.performed) //if reel button is pressed then reel the hook up
        {
                currentState = HookState.reeling;
                moveInput.y = reelSpeed;
        }
        else if (value.canceled) //if reel button is released then stop reeling
        {
            currentState = HookState.dropping;
            if (dropAmount >= maxDropAmount) //if at max drop amount, stop dropping
            {
                moveInput.y = 0f;
                return;
            }
            moveInput.y = ambientDropSpeed; //else resume ambient drop
        }

    }

    public bool getFishOnHook() => fishOnHook;
    
    public void setFishOnHook(bool value) => fishOnHook = value ? true : false;
}
