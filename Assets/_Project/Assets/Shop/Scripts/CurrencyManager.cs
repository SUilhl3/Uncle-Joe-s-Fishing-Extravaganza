using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("Starting Money (in cents, used if no save exists)")]
    [SerializeField] private int cents = 0;

    [Header("UI (optional, can be set per-scene)")]
    [SerializeField] private TMP_Text coinsText;

    private string CurrencyKey => $"save_{SaveManager.GetCurrentSlot()}_PlayerCents";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
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

    public void SetCents(int newAmount)
    {
        cents = Mathf.Max(0, newAmount);
        Save();
        RefreshUI();
    }

    public void Load()
    {
        cents = PlayerPrefs.GetInt(CurrencyKey, cents);
        RefreshUI();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(CurrencyKey, cents);
        PlayerPrefs.Save();
    }

    public void SetUIText(TMP_Text text)
    {
        coinsText = text;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (coinsText != null)
            coinsText.text = $"${(cents / 100f):F2}";
    }
}