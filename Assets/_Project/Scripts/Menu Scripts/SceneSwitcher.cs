using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject OptionsPanel;
    public GameObject PausePanel;

    void Start()
    {
        if (OptionsPanel != null) OptionsPanel.SetActive(false);
        if (PausePanel != null) PausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Load Menu");
    }

    public void OptionsMenuOpen()
    {
        if (OptionsPanel != null) OptionsPanel.SetActive(true);
    }

    public void OptionsMenuClose()
    {
        if (OptionsPanel != null) OptionsPanel.SetActive(false);
    }

    public void Game()
    {
        SceneManager.LoadScene("Boat Fishing");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenPanel()
    {
        if (PausePanel != null) PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePanel()
    {
        if (PausePanel != null) PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel != null && PausePanel.activeSelf) ClosePanel();
            else OpenPanel();
        }
    }
}
