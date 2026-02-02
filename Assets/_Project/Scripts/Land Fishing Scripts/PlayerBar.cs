using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBar : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] float upwardForce = 800f;
    [SerializeField] float gravity = 1200f;
    [SerializeField] float maxSpeed = 1000f;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    float velocity;
    bool isHolding;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //move up if holding space, down if not
        if (isHolding)
        {
            velocity += upwardForce * Time.deltaTime;
        }
        else
        {
            velocity -= gravity * Time.deltaTime;
        }

        //updating position of the image
        velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);
        Vector2 pos = rectTransform.anchoredPosition;
        pos.y += velocity * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        rectTransform.anchoredPosition = pos;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            isHolding = true;
        }

        if (value.canceled)
        {
            isHolding = false;
        }
    }
}
