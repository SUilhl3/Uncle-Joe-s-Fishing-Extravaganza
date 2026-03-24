using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float cameraBoundary = 0f;
    public float parallaxMultiplier = 0.5f;

    private Vector3 lastCameraPosition;
    private Vector3 startCameraPosition;
    private Vector3 startBackgroundPosition;

    public bool active = false;
    private bool offsetCaptured = false;

    void Start()
    {
        if (cameraTransform == null) { cameraTransform = Camera.main.transform; }

        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform.position.y < cameraBoundary) { active = true; }

        if (!active) return;

        if (!offsetCaptured)
        {
            startCameraPosition = cameraTransform.position;
            startBackgroundPosition = transform.position;
            offsetCaptured = true;
        }

        Vector3 delta = cameraTransform.position - startCameraPosition;
        transform.position = startBackgroundPosition + new Vector3(startBackgroundPosition.x, delta.y * parallaxMultiplier, 0);

        lastCameraPosition = cameraTransform.position;
    }
}