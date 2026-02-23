using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class GameManager : MonoBehaviour
{
    public Customer customer;
    public Cooking cooking;

    public List<OrderData> orders = new();

    public float orderTime = 15f;

    public float delayBetweenOrders = 3f;

    [Header("Timer UI (optional)")]
    public TextMeshProUGUI timerText;
    public Image timerFill;
    [Header("Earnings UI (optional)")]
    public TextMeshProUGUI dailyTotalText;
    [Header("Result UI (optional)")]
    public GameObject uiContainer;
    [Header("Earnings UI (optional)")]
    public TextMeshProUGUI currentTotalText;
    public TextMeshProUGUI resultText;

    [Header("Day UI (optional)")]
    public TextMeshProUGUI dayText;

    [Header("Player/Hand")]
    public Hand hand;

    float sessionTotal = 0f;

    [Header("Start Day UI (optional)")]
    public GameObject startDayPanel;
    public UnityEngine.UI.Button startDayButton;

    [Header("Pause UI (optional)")]
    public UnityEngine.UI.Button pauseButton;
    public GameObject pauseOverlay;

    [Header("Panels that pause timer")]
    public GameObject tutorialPanel;
    public GameObject pausePanel;

    float timer;
    Coroutine timerCoroutine;

    void Start()
    {
        if (startDayPanel != null)
        {
            startDayPanel.SetActive(true);
            if (uiContainer != null)
                uiContainer.SetActive(false);
            if (pauseOverlay != null)
                pauseOverlay.SetActive(false);
            Time.timeScale = 0f;
            if (startDayButton != null)
                startDayButton.onClick.AddListener(StartDay);
        }
        else
        {
            Time.timeScale = 1f;
            StartCoroutine(StartFirstOrder());
        }

        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);
    }

    IEnumerator StartFirstOrder()
    {
        yield return null;
        StartNewOrder();
    }

    public void ServeOrder()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        bool success = CheckOrder();
        if (success)
        {
            float basePrice = customer.currentOrder != null ? customer.currentOrder.basePrice : 0f;
            int secondsLeft = Mathf.CeilToInt(timer);
            secondsLeft = Mathf.Clamp(secondsLeft, 1, Mathf.CeilToInt(orderTime));
            float modifier = 1f + (secondsLeft / 100f); 
            float tip = basePrice * (modifier - 1f);
            float totalEarned = basePrice + tip;

            SaveManager.AddToDailyTotal(totalEarned);

            if (dailyTotalText != null)
            {
                float today = SaveManager.GetTodayTotal();
                dailyTotalText.text = "$" + today.ToString("F2");
            }

                sessionTotal += totalEarned;
            if (resultText != null)
                resultText.text = "$" + basePrice.ToString("F2") + " + $" + tip.ToString("F2") + " tip!";
            else if (customer != null && customer.dialogueText != null)
                customer.dialogueText.text = "$" + basePrice.ToString("F2") + " + $" + tip.ToString("F2") + " tip!";

            customer.ReactToOrder(true);

                if (currentTotalText != null)
                    currentTotalText.text = "$" + sessionTotal.ToString("F2");
        }
        else
        {
            customer.ReactToOrder(false);
            if (dailyTotalText != null)
            {
                float today = SaveManager.GetTodayTotal();
                dailyTotalText.text = "$" + today.ToString("F2");
            }
        }

        cooking.ClearPlate();
        if (hand != null)
            hand.Trash();
        UpdateTimerUI(0f, false);
        StartCoroutine(NextOrderDelay());
    }

    bool CheckOrder()
    {
        var required = customer.currentOrder?.requiredIngredients;
        var player = cooking.currentIngredients;

        var requiredMethods = customer.currentOrder?.requiredCookingMethods;

        if (required == null)
            return false;

        if (required.Count != player.Count)
            return false;

        var used = new bool[player.Count];
        for (int i = 0; i < required.Count; i++)
        {
            var reqType = required[i];
            var reqMethod = CookingMethod.Raw;
            if (requiredMethods != null && i < requiredMethods.Count)
                reqMethod = requiredMethods[i];

            bool matched = false;
            for (int j = 0; j < player.Count; j++)
            {
                if (used[j]) continue;
                var p = player[j];
                if (p == null) continue;
                if (p.type == reqType)
                {
                    if (reqMethod == CookingMethod.Raw || p.method == reqMethod)
                    {
                        used[j] = true;
                        matched = true;
                        break;
                    }
                }
            }
            if (!matched) return false;
        }

        return true;
    }

    void StartNewOrder()
    {
        if (orders != null && orders.Count > 0)
        {
            int totalWeight = 0;
            foreach (var o in orders)
                totalWeight += Mathf.Max(0, o != null ? o.rarityPercentage : 0);

            if (totalWeight > 0)
            {
                int r = Random.Range(0, totalWeight);
                int acc = 0;
                foreach (var o in orders)
                {
                    int w = Mathf.Max(0, o != null ? o.rarityPercentage : 0);
                    acc += w;
                    if (r < acc)
                    {
                        customer.currentOrder = o;
                        break;
                    }
                }
            }
            else
            {
                int idx = Random.Range(0, orders.Count);
                customer.currentOrder = orders[idx];
            }
        }

        customer.GiveOrder();
        cooking.ClearPlate();
        if (hand != null)
            hand.Trash();

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(OrderTimer());
    }

    IEnumerator OrderTimer()
    {
        timer = orderTime;
        UpdateTimerUI(timer, true);

        while (timer > 0f)
        {
            if ((tutorialPanel != null && tutorialPanel.activeSelf) ||
                (pausePanel != null && pausePanel.activeSelf))
            {
                yield return null;
                continue;
            }

            timer -= Time.deltaTime;
            UpdateTimerUI(timer, true);
            yield return null;
        }

        timer = 0f;
        timerCoroutine = null;

        customer.ReactToOrder(false);
        cooking.ClearPlate();
        if (hand != null)
            hand.Trash();
        UpdateTimerUI(0f, false);
        StartCoroutine(NextOrderDelay());
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame)
            TogglePause();
#else
        if (Input.GetKeyDown(KeyCode.LeftShift))
            TogglePause();
#endif
    }

    public void StartDay()
    {
        SaveManager.AdvanceGameDate(1);
        SaveManager.AdvanceDayNumber(1);
        if (startDayPanel != null)
            startDayPanel.SetActive(false);
        if (uiContainer != null)
            uiContainer.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(StartFirstOrder());

        sessionTotal = 0f;
        if (dailyTotalText != null)
            dailyTotalText.text = "$" + SaveManager.GetTodayTotal().ToString("F2");
        if (currentTotalText != null)
            currentTotalText.text = "$" + sessionTotal.ToString("F2");
        if (dayText != null)
            dayText.text = "Day" + SaveManager.GetDayNumber();
    }

    bool isPaused = false;
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseOverlay != null)
                pauseOverlay.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseOverlay != null)
                pauseOverlay.SetActive(false);
        }
    }

    IEnumerator NextOrderDelay()
    {
        yield return new WaitForSeconds(delayBetweenOrders);
        StartNewOrder();
    }

    void UpdateTimerUI(float t, bool visible)
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(visible);
            if (visible)
                timerText.text = Mathf.CeilToInt(t).ToString();
        }

        if (timerFill != null)
        {
            timerFill.gameObject.SetActive(visible);
            if (visible && orderTime > 0f)
                timerFill.fillAmount = Mathf.Clamp01(t / orderTime);
            else if (timerFill != null)
                timerFill.fillAmount = 0f;
        }
    }
}
