using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [Header("Character Stats")]
    public int vitality = 1;
    public int resistance = 1;
    public int endurance = 1;
    public int strength = 1;

    [Header("Blocking Absorptions")]
    public float blockingDamageAbsorption;

    public int CalculateStaminaBasedOnEnduranceLevel()
    {

        float stamina = endurance * 100;

        return Mathf.RoundToInt(stamina);
    }

    public int CalculateHealthBasedOnVitalityLevel()
    {
        float health = vitality * 100;

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
