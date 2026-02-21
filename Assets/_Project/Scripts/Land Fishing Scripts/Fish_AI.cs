using UnityEngine;

public class Fish_AI : MonoBehaviour
{
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public float directionChangeInterval = 1.5f;
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
        float currentY = rectTransform.anchoredPosition.y;

        if (currentY > maxY - 10f)
        {
            currentDirection = -1f;
        }else if (currentY < minY + 10f)
        {
            currentDirection = 1f;
        }
        else
        {
            int rand = Random.Range(0, 3);
            currentDirection = rand - 1;
        }

        directionTimer = Random.Range(0.5f, directionChangeInterval);
    }

    //moves the item randomly up or down for the fishing mini-game
    public void RandomMove()
    {
        float centreY = (minY + maxY) * 0.5f;
        float distanceFromCentre = rectTransform.anchoredPosition.y - centreY;
        
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
