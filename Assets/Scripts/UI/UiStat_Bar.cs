using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiStat_Bar : MonoBehaviour
{
    protected Slider slider;

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
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
    }
}
