using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiStat_Bar : MonoBehaviour
{
    protected Slider slider;

    private RectTransform rectTransform;

    [SerializeField] protected bool scaleBarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1f;

    public int visualMax = 1000; 

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {

    }

    public virtual void SetStat(float newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;

        int displayValue = Mathf.Min(maxValue, visualMax);
        rectTransform.sizeDelta = new Vector2(displayValue * widthScaleMultiplier, rectTransform.sizeDelta.y);

        PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
    }
}
