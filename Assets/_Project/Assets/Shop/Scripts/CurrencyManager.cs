using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("Starting Money (in cents, used if no save exists)")]
    [SerializeField] private int cents = 0; // 100 = $1.00

    [Header("UI (optional, can be set per-scene)")]
    [SerializeField] private TMP_Text coinsText;

    private const string CurrencyKey = "PlayerCents";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        cents = PlayerPrefs.GetInt(CurrencyKey, cents);
        RefreshUI();
    }

    public int Cents => cents;

    public bool CanAfford(int costInCents) => cents >= costInCents;

    public bool Spend(int costInCents)
    {
        if (!CanAfford(costInCents)) return false;
        cents -= costInCents;
        Save();
        RefreshUI();
        return true;
    }

    public void AddCents(int amountInCents)
    {
        if (amountInCents <= 0) return;
        cents += amountInCents;
        Save();
        RefreshUI();
    }

    public void SetUIText(TMP_Text text)
    {
        coinsText = text;
        RefreshUI();
    }

    private void Save()
    {
        PlayerPrefs.SetInt(CurrencyKey, cents);
        PlayerPrefs.Save();
    }

    private void RefreshUI()
    {
        if (coinsText != null)
            coinsText.text = $"${(cents / 100f):F2}";
    }
}