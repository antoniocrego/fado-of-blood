using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMatchScrollWheelToObject : MonoBehaviour
{
    [SerializeField] GameObject currentObject;
    [SerializeField] GameObject previousObject;
    [SerializeField] RectTransform currentSelectedTransform;
    [SerializeField] RectTransform contentPanel;
    [SerializeField] ScrollRect scrollRect;

    private void Update()
    {
        currentObject = EventSystem.current.currentSelectedGameObject;

        if (currentObject != null)
        {
            if (currentObject != previousObject)
            {
                previousObject = currentObject;
                currentSelectedTransform = currentObject.GetComponent<RectTransform>();
                SnapTo(currentSelectedTransform);
            }
        }
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 newPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) -
                              (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

        newPosition.x = 0;
        contentPanel.anchoredPosition = newPosition;
    }
}
