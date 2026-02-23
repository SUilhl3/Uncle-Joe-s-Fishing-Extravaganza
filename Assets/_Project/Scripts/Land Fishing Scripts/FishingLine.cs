using UnityEngine;

public class FishingLine : MonoBehaviour
{
    public Transform rodTip;
    public Transform bob;
    public LineRenderer line;
    void Start()
    {
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, rodTip.position);
        line.SetPosition(1, bob.position);
    }
}
