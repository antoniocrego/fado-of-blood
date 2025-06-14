using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{

    [Header("Blocking Absorptions")]
    public float blockingDamageAbsorption;

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {

        float stamina = endurance * 100;

        return Mathf.RoundToInt(stamina);
    }
    
    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = vitality * 10;

        return Mathf.RoundToInt(health);
    }
}
