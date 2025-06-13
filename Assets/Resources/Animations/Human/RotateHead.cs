using UnityEngine;

public class RotateHead : MonoBehaviour
{
    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(21.814f, -40.573f, -16.013f);
    }
}
