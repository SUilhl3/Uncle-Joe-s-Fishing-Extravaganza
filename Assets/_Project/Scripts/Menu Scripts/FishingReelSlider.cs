using UnityEngine;
using UnityEngine.UI;

public class FishingReelSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public RectTransform reel;

    public float spinsPerUnit = 3f;
    public bool clockwise = true;

    void Update()
    {
        if (volumeSlider == null || reel == null) return;

        float rotationZ = volumeSlider.value * 360f * spinsPerUnit;
        if (!clockwise) rotationZ = -rotationZ;

        reel.localRotation = Quaternion.Euler(0f, 0f, -rotationZ);
    }
}
