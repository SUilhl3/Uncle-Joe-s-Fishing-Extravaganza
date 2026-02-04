using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public AudioMixer mixer;         
    public string parameterName;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(parameterName, 0.75f);
        slider.value = savedVolume;
        SetVolume(savedVolume);

        slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(parameterName, dB);

        PlayerPrefs.SetFloat(parameterName, value);
    }
}
