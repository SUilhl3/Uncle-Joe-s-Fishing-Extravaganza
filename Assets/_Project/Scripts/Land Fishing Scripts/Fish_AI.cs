using UnityEngine;

public class Fish_AI : MonoBehaviour
{
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float directionChangeInterval = 1.5f;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float currentDirection = 1f;
    [SerializeField] float directionTimer;
    [SerializeField] Vector2 pos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        PickNewDirection();
    }

    void PickNewDirection()
    {
        currentDirection = Random.value > 0.5f ? 1f : -1f;
        directionTimer = Random.Range(0.5f, directionChangeInterval);
    }

    //moves the item randomly up or down for the fishing mini-game
    public void RandomMove()
    {
        directionTimer -= Time.deltaTime;

        if (directionTimer <= 0f)
        {
            PickNewDirection();
        }

        pos = rectTransform.anchoredPosition;
        pos.y += currentDirection * moveSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        rectTransform.anchoredPosition = pos;

    }
}
