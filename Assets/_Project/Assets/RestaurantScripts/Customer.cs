using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    public OrderData currentOrder;

    public TextMeshProUGUI orderText;
    public TextMeshProUGUI dialogueText;

    public string happyDialogue = "Perfect! Thanks!";
    public string angryDialogue = "This is NOT what I ordered!";

    public void GiveOrder()
    {
        orderText.text = currentOrder.orderDialogue;
        dialogueText.text = "";
    }

    public void ReactToOrder(bool success)
    {
        dialogueText.text = success ? happyDialogue : angryDialogue;
    }
    void Start()
{
    GiveOrder();
}

}
