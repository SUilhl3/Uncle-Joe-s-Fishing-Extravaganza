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

    public float dropAmount = 0f;
    public float maxDropAmount = 50f; //maximum distance the hook can drop

    public bool isReeling = false;
    public bool isAtMaxDrop = false;
    public Rigidbody2D rb;

    void Start()
    {
        moveInput.y = ambientDropSpeed;
        rb = GetComponent<Rigidbody2D>();   
    }
    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x * horzSpeed, moveInput.y,0f);
        transform.Translate(movement * Time.deltaTime, Space.World);

        if(isAtMaxDrop == false && dropAmount >= maxDropAmount) //check if at max drop amount
        {
            isAtMaxDrop = true; //set flag to true to prevent rerunning this check
            moveInput.y = 0f; //stop dropping if at max drop amount
        }
        
        if(!isReeling && dropAmount <= maxDropAmount) //if not reeling, increase drop amount unless drop amount is at maxDropAmount
        {
            dropAmount += -moveInput.y * Time.deltaTime;
        }
    }

    //input functions
    public void OnMove(InputAction.CallbackContext value) //handles horizontal movement
    {
            Vector2 input = value.ReadValue<Vector2>();
            moveInput.x = input.x;
    }

    public void OnReel(InputAction.CallbackContext value)
    {
        if(value.performed) //if reel button is pressed then reel the hook up
        {
            isReeling = true;
            moveInput.y = reelSpeed;
        }
        else if(value.canceled) //if reel button is released then stop reeling
        {
            isReeling = false;
            if(dropAmount >= maxDropAmount) //if at max drop amount, stop dropping
            {
                moveInput.y = 0f;
                return;
            }
            moveInput.y = ambientDropSpeed; //else resume ambient drop
        }
    }

}
