using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    float timer;
    Coroutine timerCoroutine;

    void Start()
    {
        StartCoroutine(StartFirstOrder());
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
        customer.ReactToOrder(success);
        cooking.ClearPlate();
        UpdateTimerUI(0f, false);
        StartCoroutine(NextOrderDelay());
    }

    bool CheckOrder()
    {
        var required = customer.currentOrder?.requiredIngredients;
        var player = cooking.currentIngredients;

        if (required == null)
            return false;

        if (required.Count != player.Count)
            return false;

        foreach (var ingredient in required)
        {
            if (!player.Contains(ingredient))
                return false;
        }

        return true;
    }

    void StartNewOrder()
    {
        if (orders != null && orders.Count > 0)
        {
            int idx = Random.Range(0, orders.Count);
            customer.currentOrder = orders[idx];
        }

        customer.GiveOrder();
        cooking.ClearPlate();

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
            timer -= Time.deltaTime;
            UpdateTimerUI(timer, true);
            yield return null;
        }

        timer = 0f;
        timerCoroutine = null;

        customer.ReactToOrder(false);
        cooking.ClearPlate();
        UpdateTimerUI(0f, false);
        StartCoroutine(NextOrderDelay());
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
