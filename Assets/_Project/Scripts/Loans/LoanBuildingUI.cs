using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoanBuildingUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text debtText;
    [SerializeField] private TMP_Text weeklyMinimumText;
    [SerializeField] private TMP_Text weekStatusText;
    [SerializeField] private TMP_Text resultText;

    [Header("Buttons")]
    [SerializeField] private Button payMinimumButton;
    [SerializeField] private Button payExtraButton;
    [SerializeField] private Button backButton;

    [Header("Extra Payment")]
    [SerializeField] private TMP_InputField extraPaymentInput;

    [Header("Scene Flow")]
    [SerializeField] private string mapSceneName = "Map";
    [SerializeField] private string loseSceneName = "Main Menu";

    void Start()
    {
        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (payMinimumButton != null)
            payMinimumButton.onClick.AddListener(PayMinimum);

        if (payExtraButton != null)
            payExtraButton.onClick.AddListener(PayExtra);

        if (backButton != null)
            backButton.onClick.AddListener(BackToMap);

        RefreshUI();
        CheckImmediateLoseCondition();
    }

    void RefreshUI()
    {
        if (LoanManager.Instance == null)
            return;

        if (debtText != null)
            debtText.text = "Debt: " + LoanManager.Instance.GetDebtDisplay();

        if (weeklyMinimumText != null)
            weeklyMinimumText.text = "Weekly Payment: " + LoanManager.Instance.GetMinimumDisplay();

        if (weekStatusText != null)
        {
            if (LoanManager.Instance.IsDebtCleared())
                weekStatusText.text = "Loan fully paid off!";
            else if (LoanManager.Instance.IsPaidForCurrentWeek())
                weekStatusText.text = "This week's payment is already paid.";
            else
                weekStatusText.text = "Payment due this week.";
        }

        if (payMinimumButton != null)
            payMinimumButton.interactable = !LoanManager.Instance.IsPaidForCurrentWeek() &&
                                           !LoanManager.Instance.IsDebtCleared() &&
                                           LoanManager.Instance.CanAffordMinimum();

        if (payExtraButton != null)
            payExtraButton.interactable = !LoanManager.Instance.IsDebtCleared();
    }

    void CheckImmediateLoseCondition()
    {
        if (LoanManager.Instance == null || CurrencyManager.Instance == null)
            return;

        if (LoanManager.Instance.IsPaymentDueThisWeek() && !LoanManager.Instance.CanAffordMinimum())
        {
            if (resultText != null)
                resultText.text = "You couldn't make the weekly payment. You lose.";

            Invoke(nameof(GoToLoseScene), 2f);
        }
    }

    public void PayMinimum()
    {
        if (LoanManager.Instance == null)
            return;

        if (LoanManager.Instance.IsPaidForCurrentWeek())
        {
            if (resultText != null)
                resultText.text = "This week's payment is already paid.";
            RefreshUI();
            return;
        }

        bool success = LoanManager.Instance.PayMinimumForCurrentWeek();

        if (resultText != null)
            resultText.text = success ? "Minimum payment made." : "Not enough money for minimum payment.";

        RefreshUI();
    }

    public void PayExtra()
    {
        if (LoanManager.Instance == null || extraPaymentInput == null)
            return;

        if (LoanManager.Instance.IsDebtCleared())
        {
            if (resultText != null)
                resultText.text = "The loan is already paid off.";
            return;
        }

        if (!float.TryParse(extraPaymentInput.text, out float dollars))
        {
            if (resultText != null)
                resultText.text = "Enter a valid dollar amount.";
            return;
        }

        int cents = Mathf.RoundToInt(dollars * 100f);

        bool success = LoanManager.Instance.PayExtra(cents);

        if (resultText != null)
            resultText.text = success ? "Extra payment made." : "Could not make extra payment.";

        RefreshUI();
    }

    public void BackToMap()
    {
        SceneManager.LoadScene(mapSceneName);
    }

    void GoToLoseScene()
    {
        SceneManager.LoadScene(loseSceneName);
    }
}