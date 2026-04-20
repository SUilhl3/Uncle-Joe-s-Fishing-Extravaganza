using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TreasureBuyerVoicePlayer : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] voiceLines;

    [Header("Options")]
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool randomizePitch = false;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (playOnStart)
            PlayRandomVoiceLine();
    }

    public void PlayRandomVoiceLine()
    {
        if (voiceLines == null || voiceLines.Length == 0)
        {
            Debug.LogWarning("Treasure buyer has no voice lines assigned.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource missing on TreasureBuyerVoicePlayer.");
            return;
        }

        int randomIndex = Random.Range(0, voiceLines.Length);
        AudioClip clip = voiceLines[randomIndex];

        if (clip == null)
            return;

        if (randomizePitch)
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        else
            audioSource.pitch = 1f;

        audioSource.clip = clip;
        audioSource.Play();
    }
}