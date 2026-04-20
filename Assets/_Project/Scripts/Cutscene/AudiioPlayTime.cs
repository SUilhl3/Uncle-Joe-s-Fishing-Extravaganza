using UnityEngine;

public class AudiioPlayTime : MonoBehaviour
{
    public AudioSource audioSource;

    public void PLayAudio()
    {
        audioSource.Play();
    }
}
