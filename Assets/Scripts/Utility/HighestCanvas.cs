using UnityEngine;

public class HighestCanvas : MonoBehaviour
{
    [SerializeField] int sortingOrder = 9999;
    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = sortingOrder;
    }
}
