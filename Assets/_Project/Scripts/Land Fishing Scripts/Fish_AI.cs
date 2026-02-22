using UnityEngine;

public class Fish_AI : MonoBehaviour
{
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] public float moveSpeed = 1.0f;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Vector2 pos;
    public FishDifficulty difficulty;
    [SerializeField] float smoothSpeed = 8f;
    [SerializeField] float noiseFrequency = 0.5f;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    //moves the item randomly up or down for the fishing mini-game
    public void RandomMove()
    {
        //sets move speed based on difficulty 
       switch(difficulty)
        {
            case FishDifficulty.EASY:
                moveSpeed = 1.0f;
                break;
            case FishDifficulty.MEDIUM:
                moveSpeed = 1.5f;
                break;
            case FishDifficulty.HARD:
                moveSpeed = 2.0f;
                break;
            case FishDifficulty.LEGENDARY:
                moveSpeed = 2.5f;
                break;
        }

        
        float noise = Mathf.PerlinNoise1D(Time.time * noiseFrequency * moveSpeed);

        float targetY = Mathf.Lerp(minY, maxY, noise);

        pos = rectTransform.anchoredPosition;

        
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * smoothSpeed);

        rectTransform.anchoredPosition = pos;

    }

}
