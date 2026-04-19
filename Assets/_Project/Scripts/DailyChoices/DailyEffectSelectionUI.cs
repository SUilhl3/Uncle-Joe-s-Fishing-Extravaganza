using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DailyEffectSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private DailyEffectCardUI cardPrefab;
    [SerializeField] private string nextSceneName = "Map";

    private void Start()
    {
        BuildCards();
    }

    void BuildCards()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        List<DailyEffectData> choices = DailyEffectManager.Instance.GetOrGenerateChoicesForToday();

        foreach (var choice in choices)
        {
            DailyEffectCardUI card = Instantiate(cardPrefab, contentRoot);
            card.Setup(choice, OnCardChosen);
        }
    }

    void OnCardChosen(DailyEffectData chosen)
    {
        DailyEffectManager.Instance.ChooseEffect(chosen);
        SceneManager.LoadScene(nextSceneName);
    }
}