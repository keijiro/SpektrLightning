using UnityEngine;

public class Tester : MonoBehaviour
{
    void Update()
    {
        var lr = GetComponent<Spektr.LightningRenderer>();
        lr.throttle = Input.GetMouseButton(0) ? 0.4f : 0.045f;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 200), "Press mouse button to burst.");
    }
}
