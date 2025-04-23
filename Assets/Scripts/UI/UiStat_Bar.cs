using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiStat_Bar : MonoBehaviour
{
    private Slider slider; 


    protected virtual void Awake() 
    {
        slider = GetComponent<Slider>();
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
