using UnityEngine;
using TMPro;

public class CurrencyUIBinder : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    private void Start()
    {
        if (currencyText == null)
            currencyText = GetComponent<TMP_Text>();

        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.SetUIText(currencyText);
    }
}