using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OptionsMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Game(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
