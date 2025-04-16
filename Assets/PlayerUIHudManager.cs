using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UiStat_Bar staminaBar; 

    public void SetNewStaminaValue(float newValue) 
    {
        staminaBar.SetStat(newValue);
    }

    public void SetMaxStaminaValue(int newValue) 
    {
        staminaBar.SetMaxStat(newValue);
    }
}
