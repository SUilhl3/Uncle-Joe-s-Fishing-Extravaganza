using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DailyEffectSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private DailyEffectCardUI cardPrefab;
    [SerializeField] private string nextSceneName = "Map";
    [SerializeField] private Button rerollButton;

    private bool hasUsedReroll = false;

    private void Start()
    {
        BuildCards();

        if (rerollButton != null)
        {
            rerollButton.onClick.RemoveAllListeners();
            rerollButton.onClick.AddListener(RerollCards);

            bool hasRerollUpgrade = SaveManager.GetSlotInt("UhhhYesPurchased", 0) == 1;
            rerollButton.gameObject.SetActive(hasRerollUpgrade);
            rerollButton.interactable = hasRerollUpgrade;
        }
    }

    void BuildCards()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        if (DailyEffectManager.Instance == null)
        {
            Debug.LogWarning("DailyEffectManager.Instance is null.");
            return;
        }

        List<DailyEffectData> choices = DailyEffectManager.Instance.GetOrGenerateChoicesForToday();

        foreach (var choice in choices)
        {
            DailyEffectCardUI card = Instantiate(cardPrefab, contentRoot);
            card.Setup(choice, OnCardChosen);
        }
    }

    void OnCardChosen(DailyEffectData chosen)
    {
        if (DailyEffectManager.Instance == null)
        {
            Debug.LogWarning("DailyEffectManager.Instance is null.");
            return;
        }

        DailyEffectManager.Instance.ChooseEffect(chosen);

        SaveManager.StartNewDay();

        if (LoanManager.Instance != null && LoanManager.Instance.IsPaymentDueThisWeek())
        {
            SceneManager.LoadScene("LoanBuildingScene");
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void RerollCards()
    {
        if (hasUsedReroll)
            return;

        if (SaveManager.GetSlotInt("UhhhYesPurchased", 0) != 1)
            return;

        hasUsedReroll = true;

        string offerKey = SaveManager.SlotKey("DailyEffectOffers_" + SaveManager.GetCurrentGameDate().ToString("yyyyMMdd"));
        PlayerPrefs.DeleteKey(offerKey);
        PlayerPrefs.Save();

        BuildCards();

        if (rerollButton != null)
            rerollButton.interactable = false;
    }
}