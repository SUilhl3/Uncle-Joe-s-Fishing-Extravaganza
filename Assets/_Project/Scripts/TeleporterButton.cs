using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterButton : MonoBehaviour
{
    private const string TeleporterKey = "TeleporterPurchased";

    [SerializeField] private GameObject buttonRoot;
    [SerializeField] private string restaurantSceneName = "Restaurant";

    void Start()
    {
        if (PlayerPrefs.GetInt(TeleporterKey, 0) == 1)
        {
            buttonRoot.SetActive(true);
        }
        else
        {
            buttonRoot.SetActive(false);
        }
    }

    public void Teleport()
    {
        SceneManager.LoadScene(restaurantSceneName);
    }
}