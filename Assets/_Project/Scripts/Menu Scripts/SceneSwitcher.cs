using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public GameObject OptionsPanel;
    public GameObject PausePanel;

    void Start()
    {
        optionsPanel.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Load Menu");
    }

    public void OptionsMenuOpen()
    {
        OptionsPanel.SetActive(true);
    }

    public void OptionsMenuClose()
    {
       OptionsPanel.SetActive(false);
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
        PausePanel.SetActive(true);
        set timeScale to 0f;
    }

    public void ClosePanel()
    {
        PausePanel.SetActive(false);
        set timeScale to 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanel.SetActive(!panel.activeSelf);
        }
    }

    


}
