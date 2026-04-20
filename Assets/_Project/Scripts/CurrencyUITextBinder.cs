using TMPro;
using UnityEngine;

public class CurrencyUITextBinder : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    private void Awake()
    {
        if (moneyText == null)
            moneyText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Debug.Log("CurrencyUITextBinder Start on scene object: " + gameObject.name);
        Debug.Log("CurrencyManager.Instance exists? " + (CurrencyManager.Instance != null));

        if (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("CurrencyManager.Instance is null. Make sure CurrencyManager exists before this scene loads.");
            return;
        }

        if (moneyText == null)
        {
            Debug.LogWarning("CurrencyUITextBinder has no TMP_Text assigned.");
            return;
        }

        Debug.Log("Binding money text. Current cents = " + CurrencyManager.Instance.Cents);

        CurrencyManager.Instance.SetUIText(moneyText);
    }
}