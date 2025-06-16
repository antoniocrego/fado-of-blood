using UnityEngine;

public class HighestCanvas : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 9999;
    }
}
