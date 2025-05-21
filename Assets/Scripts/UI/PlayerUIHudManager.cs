using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{

    [SerializeField] UiStat_Bar healthBar; 
    [SerializeField] UiStat_Bar staminaBar; 
    
    public void SetNewHealthValue(float newValue) 
    {
        healthBar.SetStat(newValue);
    }   

    public void SetMaxHealthValue(int newValue) 
    {
        healthBar.SetMaxStat(newValue);
    }

    public void SetNewStaminaValue(float newValue)
    {
        staminaBar.SetStat(newValue);
    }   

    public void SetMaxStaminaValue(int newValue) 
    {
        staminaBar.SetMaxStat(newValue);
    }
}
