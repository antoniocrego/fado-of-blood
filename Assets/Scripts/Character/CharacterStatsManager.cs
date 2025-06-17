using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [Header("Character Stats")]
    public int vitality = 1;
    public int resistance = 1;
    public int endurance = 1;
    public int strength = 1;
    public int scaleFactor = 10; 

    [Header("Blocking Absorptions")]
    public float blockingDamageAbsorption;
    public float blockingStability;

    public int CalculateStaminaBasedOnEnduranceLevel()
    {

        float maxStaminaCap = 1000.0f;
        float exponentValue = -(float)endurance / scaleFactor;
        float stamina = maxStaminaCap * (1f - Mathf.Exp(exponentValue));
        return Mathf.RoundToInt(stamina);
    }

    public int CalculateHealthBasedOnVitalityLevel()
    {
        float maxHealthCap = 1600.0f; 
        float exponentValue = -(float)vitality / scaleFactor;
        float health = maxHealthCap * (1f - Mathf.Exp(exponentValue));
        return Mathf.RoundToInt(health);
    }
    public int CalculateCharacterLevel(bool expectedLevel = false)
    {
        int characterLevel;

        if (expectedLevel)
        {
            PlayerUILevelUpManager playerLevelUpManager = PlayerUIManager.instance.playerUILevelUpManager;
            characterLevel = Mathf.RoundToInt(playerLevelUpManager.vigorSlider.value +
                                          playerLevelUpManager.resistanceSlider.value +
                                          playerLevelUpManager.enduranceSlider.value +
                                          playerLevelUpManager.strengthSlider.value);
        }
        else
        {
            characterLevel = vitality + resistance + endurance + strength;
        }

        characterLevel = characterLevel - 4 + 1; // level starts counting past the first 4 attributes

        return characterLevel;
    }
}
