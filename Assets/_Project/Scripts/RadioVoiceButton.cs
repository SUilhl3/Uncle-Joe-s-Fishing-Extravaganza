using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class RadioVoiceButton : MonoBehaviour
{
    private const string RadioPurchasedKey = "RadioPurchased";

    [Header("UI")]
    [SerializeField] private GameObject buttonRoot;
    [SerializeField] private Button radioButton;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] voiceLines;

    [Header("Options")]
    [SerializeField] private bool stopCurrentClipBeforePlaying = true;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (radioButton != null)
        {
            radioButton.onClick.RemoveListener(PlayRandomVoiceLine);
            radioButton.onClick.AddListener(PlayRandomVoiceLine);
        }
    }

    private void Start()
    {
        RefreshVisibility();
    }

    public void RefreshVisibility()
    {
        bool unlocked = PlayerPrefs.GetInt(RadioPurchasedKey, 0) == 1;

        if (buttonRoot != null)
            buttonRoot.SetActive(unlocked);
    }

    public void PlayRandomVoiceLine()
    {
        if (voiceLines == null || voiceLines.Length == 0)
        {
            Debug.LogWarning("Radio has no voice lines assigned.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("Radio AudioSource is missing.");
            return;
        }

        int randomIndex = Random.Range(0, voiceLines.Length);
        AudioClip chosenClip = voiceLines[randomIndex];

        if (chosenClip == null)
        {
            Debug.LogWarning("Chosen radio voice line is null.");
            return;
        }

        if (stopCurrentClipBeforePlaying && audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = chosenClip;
        audioSource.Play();
    }
}