using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingUIButtonManager : MonoBehaviour
{
    public Button fryerButton;
    public Button ovenButton;
    public Button cuttingButton;

    string fryerOriginal;
    string ovenOriginal;
    string cuttingOriginal;

    void Start()
    {
        fryerOriginal = GetButtonLabelText(fryerButton) ?? "Fry";
        ovenOriginal = GetButtonLabelText(ovenButton) ?? "Bake";
        cuttingOriginal = GetButtonLabelText(cuttingButton) ?? "Cut";

        if (fryerButton != null) fryerButton.onClick.AddListener(OnFryerButtonClicked);
        if (ovenButton != null) ovenButton.onClick.AddListener(OnOvenButtonClicked);
        if (cuttingButton != null) cuttingButton.onClick.AddListener(OnCuttingButtonClicked);

        CookingStationEvents.OnStationStateChanged += OnStationStateChanged;
    }

    void OnDestroy()
    {
        CookingStationEvents.OnStationStateChanged -= OnStationStateChanged;
    }

    void OnStationStateChanged(StationType type, StationState state)
    {
        switch (type)
        {
            case StationType.Fryer:
                switch (state)
                {
                    case StationState.Cooking: SetButtonLabel(fryerButton, "Frying"); break;
                    case StationState.Ready: SetButtonLabel(fryerButton, "Ready"); break;
                    case StationState.Burnt: SetButtonLabel(fryerButton, "Burnt"); break;
                }
                break;
            case StationType.Oven:
                switch (state)
                {
                    case StationState.Cooking: SetButtonLabel(ovenButton, "Baking"); break;
                    case StationState.Ready: SetButtonLabel(ovenButton, "Ready"); break;
                    case StationState.Burnt: SetButtonLabel(ovenButton, "Burnt"); break;
                }
                break;
            case StationType.Cutting:
                switch (state)
                {
                    case StationState.Cooking: SetButtonLabel(cuttingButton, "Cutting"); break;
                    case StationState.Ready: SetButtonLabel(cuttingButton, "Ready"); break;
                    case StationState.Burnt: SetButtonLabel(cuttingButton, "Burnt"); break;
                }
                break;
        }
    }

    void SetButtonLabel(Button b, string label)
    {
        if (b == null) return;
        var tmp = b.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = label;
            return;
        }

        var t = b.GetComponentInChildren<UnityEngine.UI.Text>();
        if (t != null)
            t.text = label;
    }

    string GetButtonLabelText(Button b)
    {
        if (b == null) return null;
        var tmp = b.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) return tmp.text;
        var t = b.GetComponentInChildren<UnityEngine.UI.Text>();
        if (t != null) return t.text;
        return null;
    }


    public void OnFryerButtonClicked()
    {
        var label = GetButtonLabelText(fryerButton);
        if (label == "Ready" || label == "Burnt") SetButtonLabel(fryerButton, fryerOriginal);
    }

    public void OnOvenButtonClicked()
    {
        var label = GetButtonLabelText(ovenButton);
        if (label == "Ready" || label == "Burnt") SetButtonLabel(ovenButton, ovenOriginal);
    }

    public void OnCuttingButtonClicked()
    {
        var label = GetButtonLabelText(cuttingButton);
        if (label == "Ready" || label == "Burnt") SetButtonLabel(cuttingButton, cuttingOriginal);
    }
}
