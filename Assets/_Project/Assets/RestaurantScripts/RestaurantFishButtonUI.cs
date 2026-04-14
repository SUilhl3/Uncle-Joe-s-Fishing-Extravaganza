using UnityEngine;
using UnityEngine.UI;

public class RestaurantFishButtonUI : MonoBehaviour
{
    [SerializeField] private Button smallFishButton;
    [SerializeField] private Button mediumFishButton;
    [SerializeField] private Button largeFishButton;

    void Start()
    {
        RefreshButtons();
    }

    void Update()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        if (smallFishButton != null)
            smallFishButton.interactable = CatchInventoryManager.GetFishCountBySize(FishSize.Small) > 0;

        if (mediumFishButton != null)
            mediumFishButton.interactable = CatchInventoryManager.GetFishCountBySize(FishSize.Medium) > 0;

        if (largeFishButton != null)
            largeFishButton.interactable = CatchInventoryManager.GetFishCountBySize(FishSize.Large) > 0;
    }
}