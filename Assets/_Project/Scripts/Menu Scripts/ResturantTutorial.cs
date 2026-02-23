using UnityEngine;

public class ResturantTutorial : MonoBehaviour
{
    public GameObject TutorialPanel;

    void Start()
    {
        if (TutorialPanel != null) TutorialPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseTutorial()
    {
        if (TutorialPanel != null) TutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
