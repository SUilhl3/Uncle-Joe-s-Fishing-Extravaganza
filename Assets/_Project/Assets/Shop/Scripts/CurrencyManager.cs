using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int coins = 100;
    [SerializeField] private TMP_Text coinsText;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        RefreshUI();
    }

    public int Coins => coins;

    public bool CanAfford(int cost) => coins >= cost;

    public bool Spend(int cost)
    {
        if (!CanAfford(cost)) return false;
        coins -= cost;
        RefreshUI();
        return true;
    }

    private void RefreshUI()
    {
        if (coinsText != null) coinsText.text = coins.ToString();
    }
}