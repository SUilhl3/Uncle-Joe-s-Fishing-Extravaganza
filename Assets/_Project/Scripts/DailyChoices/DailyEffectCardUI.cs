using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyEffectCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button chooseButton;

    private DailyEffectData currentData;
    private Action<DailyEffectData> onChosen;

    public void Setup(DailyEffectData data, Action<DailyEffectData> callback)
    {
        currentData = data;
        onChosen = callback;

        if (titleText != null) titleText.text = data.effectName;
        if (descText != null) descText.text = data.description;
        if (iconImage != null) iconImage.sprite = data.icon;

        if (chooseButton != null)
        {
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(Choose);
        }
    }

    void Choose()
    {
        onChosen?.Invoke(currentData);
    }
}