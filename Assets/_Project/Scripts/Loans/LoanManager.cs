using UnityEngine;

public class LoanManager : MonoBehaviour
{
    public static LoanManager Instance { get; private set; }

    [Header("Starting Loan Settings")]
    [SerializeField] private int startingDebtInCents = 500000; // $5000.00
    [SerializeField] private int weeklyMinimumPaymentInCents = 25000; // $250.00

    private const string DebtKey = "LoanDebtInCents";
    private const string WeeklyMinimumKey = "LoanWeeklyMinimumInCents";
    private const string LastPaidWeekKey = "LoanLastPaidWeek";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeIfNeeded();
    }

    void InitializeIfNeeded()
    {
        if (!PlayerPrefs.HasKey(DebtKey))
            PlayerPrefs.SetInt(DebtKey, startingDebtInCents);

        if (!PlayerPrefs.HasKey(WeeklyMinimumKey))
            PlayerPrefs.SetInt(WeeklyMinimumKey, weeklyMinimumPaymentInCents);

        if (!PlayerPrefs.HasKey(LastPaidWeekKey))
            PlayerPrefs.SetInt(LastPaidWeekKey, 0);

        PlayerPrefs.Save();
    }

    public int GetDebtInCents()
    {
        return PlayerPrefs.GetInt(DebtKey, startingDebtInCents);
    }

    public int GetWeeklyMinimumInCents()
    {
        return PlayerPrefs.GetInt(WeeklyMinimumKey, weeklyMinimumPaymentInCents);
    }

    public int GetCurrentWeekNumber()
    {
        int day = SaveManager.GetDayNumber();
        return Mathf.CeilToInt(day / 7f);
    }

    public int GetLastPaidWeek()
    {
        return PlayerPrefs.GetInt(LastPaidWeekKey, 0);
    }

    public bool IsPaidForCurrentWeek()
    {
        return GetLastPaidWeek() >= GetCurrentWeekNumber();
    }

    public bool IsDebtCleared()
    {
        return GetDebtInCents() <= 0;
    }

    public bool IsPaymentDueThisWeek()
    {
        if (IsDebtCleared())
            return false;

        return !IsPaidForCurrentWeek();
    }

    public bool CanAffordMinimum()
    {
        return CurrencyManager.Instance != null &&
               CurrencyManager.Instance.CanAfford(GetWeeklyMinimumInCents());
    }

    public bool PayMinimumForCurrentWeek()
    {
        if (CurrencyManager.Instance == null)
            return false;

        if (IsPaidForCurrentWeek())
            return false;

        int amount = Mathf.Min(GetWeeklyMinimumInCents(), GetDebtInCents());

        if (!CurrencyManager.Instance.Spend(amount))
            return false;

        ReduceDebt(amount);
        PlayerPrefs.SetInt(LastPaidWeekKey, GetCurrentWeekNumber());
        PlayerPrefs.Save();
        return true;
    }

    public bool PayExtra(int amountInCents)
    {
        if (amountInCents <= 0)
            return false;

        if (CurrencyManager.Instance == null)
            return false;

        int debt = GetDebtInCents();
        int payment = Mathf.Min(amountInCents, debt);

        if (!CurrencyManager.Instance.Spend(payment))
            return false;

        ReduceDebt(payment);
        return true;
    }

    void ReduceDebt(int amountInCents)
    {
        int debt = GetDebtInCents();
        debt -= amountInCents;
        debt = Mathf.Max(0, debt);

        PlayerPrefs.SetInt(DebtKey, debt);
        PlayerPrefs.Save();
    }

    public string GetDebtDisplay()
    {
        return "$" + (GetDebtInCents() / 100f).ToString("F2");
    }

    public string GetMinimumDisplay()
    {
        return "$" + (GetWeeklyMinimumInCents() / 100f).ToString("F2");
    }
}