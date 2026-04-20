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
        Time.timeScale = 0f;
    }

    public void OptionsMenuClose()
    {
        if (OptionsPanel != null) OptionsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseMenuOpen()
    {
        if (PausePanel != null) PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PauseMenuClose()
    {
        if (PausePanel != null) PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Game()
    {
        SceneManager.LoadScene("Map");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Shop()
    {
        SceneManager.LoadScene("Shop2D");
    }

    public void LandFishing()
    {
        SceneManager.LoadScene("Land Fishing");
    }

    public void Boat()
    {
        SceneManager.LoadScene("Boat Fishing");
    }

    public void Resturant()
    {
        SceneManager.LoadScene("Restaurant");
    }

    public void Inventory()
    {
        SceneManager.LoadScene("InventoryScene");
    }

    public void TreasureBuyer()
    {
        SceneManager.LoadScene("TresureBuyerScene");
    }

    public void LoanBuilding()
    {
        SceneManager.LoadScene("LoanBuildingScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}