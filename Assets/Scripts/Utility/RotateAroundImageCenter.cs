using UnityEngine;

public class RotateAroundImageCenter : MonoBehaviour
{
    public float rotationSpeed = 100f; // Degrees per second

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
