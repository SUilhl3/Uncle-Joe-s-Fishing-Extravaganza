using System.Collections;
using UnityEngine;

public class Cam_Follow_Hook : MonoBehaviour
{
    public Vector3 targetPosition;
    public GameObject target;
    public Camera cam;

    void Start()
    {
        targetPosition = target.transform.position;
        cam = Camera.main;
        cam.transform.position = new Vector3(targetPosition.x, targetPosition.y, cam.transform.position.z);
    }

    void Update()
    {
        targetPosition = target.transform.position;
        cam.transform.position = new Vector3(targetPosition.x, targetPosition.y, cam.transform.position.z);
    }
}
