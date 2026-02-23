using UnityEngine;
using TMPro;

public class TutorialTextManager : MonoBehaviour
{
    [Header("Text Component")]
    public TextMeshProUGUI targetText;

    [Header("Texts To Cycle Through")]
    [TextArea(2, 5)]
    public string[] texts;

    private int currentIndex = 0;

    void Start()
    {
        UpdateText();
    }

    public void NextText()
    {
        if (texts.Length == 0) return;

        currentIndex++;
        if (currentIndex >= texts.Length)
            currentIndex = 0;

        UpdateText();
    }

    public void PreviousText()
    {
        if (texts.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = texts.Length - 1;

        UpdateText();
    }

    private void UpdateText()
    {
        if (targetText != null && texts.Length > 0)
        {
            targetText.text = texts[currentIndex];
        }
    }
}