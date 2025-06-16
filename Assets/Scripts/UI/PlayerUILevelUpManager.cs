using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUILevelUpManager : PlayerUIMenu
{
    [Header("Levels")]
    [SerializeField] int[] playerLevels = new int[392]; // there are 392 possible levels in the game
    [SerializeField] int totalLevelUpCost = 0;

    [Header("Stats")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI bloodDropsHeldText;
    [SerializeField] TextMeshProUGUI bloodDropsNeededText;
    [SerializeField] TextMeshProUGUI vigorText;
    [SerializeField] TextMeshProUGUI resistanceText;
    [SerializeField] TextMeshProUGUI enduranceText;
    [SerializeField] TextMeshProUGUI strengthText;

    [Header("Expected Stats")]
    [SerializeField] TextMeshProUGUI expectedLevelText;
    [SerializeField] TextMeshProUGUI expectedBloodDropsHeldText;
    [SerializeField] TextMeshProUGUI expectedVigorText;
    [SerializeField] TextMeshProUGUI expectedResistanceText;
    [SerializeField] TextMeshProUGUI expectedEnduranceText;
    [SerializeField] TextMeshProUGUI expectedStrengthText;

    [Header("Sliders")]
    public Slider vigorSlider;
    public Slider resistanceSlider;
    public Slider enduranceSlider;
    public Slider strengthSlider;

    [Header("Buttons")]
    [SerializeField] Button confirmButton;

    private void Awake()
    {
        SetAllLevelsCost();
    }

    public override void OpenMenu()
    {
        base.OpenMenu();
        SetCurrentStats();
    }

    private void SetCurrentStats()
    {
        PlayerStatsManager playerStats = PlayerUIManager.instance.playerManager.playerStatsManager;
        levelText.text = playerStats.CalculateCharacterLevel().ToString();
        expectedLevelText.text = playerStats.CalculateCharacterLevel().ToString();

        bloodDropsHeldText.text = playerStats.bloodDrops.ToString();
        expectedBloodDropsHeldText.text = playerStats.bloodDrops.ToString();
        bloodDropsNeededText.text = "0";

        vigorText.text = playerStats.vitality.ToString();
        expectedVigorText.text = playerStats.vitality.ToString();
        vigorSlider.minValue = playerStats.vitality;
        vigorSlider.value = playerStats.vitality;

        resistanceText.text = playerStats.resistance.ToString();
        expectedResistanceText.text = playerStats.resistance.ToString();
        resistanceSlider.minValue = playerStats.resistance;
        resistanceSlider.value = playerStats.resistance;

        enduranceText.text = playerStats.endurance.ToString();
        expectedEnduranceText.text = playerStats.endurance.ToString();
        enduranceSlider.minValue = playerStats.endurance;
        enduranceSlider.value = playerStats.endurance;

        strengthText.text = playerStats.strength.ToString();
        expectedStrengthText.text = playerStats.strength.ToString();
        strengthSlider.minValue = playerStats.strength;
        strengthSlider.value = playerStats.strength;

        vigorSlider.Select();
        vigorSlider.OnSelect(null);
    }

    public void UpdateExpectedLevelText()
    {
        PlayerStatsManager playerStats = PlayerUIManager.instance.playerManager.playerStatsManager;
        int expectedLevel = playerStats.CalculateCharacterLevel(true);
        expectedLevelText.text = expectedLevel.ToString();

        int currentLevel = playerStats.CalculateCharacterLevel();
        totalLevelUpCost = CalculateLevelCost(currentLevel, expectedLevel);
        bloodDropsNeededText.text = totalLevelUpCost.ToString();
        int expectedBloodDropsHeld = playerStats.bloodDrops - totalLevelUpCost;
        expectedBloodDropsHeldText.text = expectedBloodDropsHeld.ToString();

        if (totalLevelUpCost > playerStats.bloodDrops)
        {
            confirmButton.interactable = false;
            return;
        }
        else
        {
            confirmButton.interactable = true;
        }

        ChangeTextColorsDependingOnCost();
    }

    public void UpdateVigorSlider()
    {
        UpdateExpectedLevelText();
        expectedVigorText.text = vigorSlider.value.ToString();
    }

    public void UpdateResistanceSlider()
    {
        UpdateExpectedLevelText();
        expectedResistanceText.text = resistanceSlider.value.ToString();
    }

    public void UpdateEnduranceSlider()
    {
        UpdateExpectedLevelText();
        expectedEnduranceText.text = enduranceSlider.value.ToString();
    }

    public void UpdateStrengthSlider()
    {
        UpdateExpectedLevelText();
        expectedStrengthText.text = strengthSlider.value.ToString();
    }

    public void ConfirmLevelUp()
    {
        if (totalLevelUpCost <= 0 || totalLevelUpCost > PlayerUIManager.instance.playerManager.playerStatsManager.bloodDrops)
        {
            return;
        }
        PlayerManager player = PlayerUIManager.instance.playerManager;

        PlayerStatsManager playerStats = player.playerStatsManager;
        playerStats.SetBloodDrops(playerStats.bloodDrops - totalLevelUpCost);

        int vigor = Mathf.RoundToInt(vigorSlider.value);
        int resistance = Mathf.RoundToInt(resistanceSlider.value);
        int endurance = Mathf.RoundToInt(enduranceSlider.value);
        int strength = Mathf.RoundToInt(strengthSlider.value);

        playerStats.vitality = vigor;
        playerStats.resistance = resistance;
        playerStats.endurance = endurance;
        playerStats.strength = strength;

        PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(player.health);
        PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(player.stamina);

        SetCurrentStats();
        ChangeTextColorsDependingOnCost();
    }

    private void SetAllLevelsCost()
    {
        for (int i = 0; i < playerLevels.Length; i++)
        {
            playerLevels[i] = Mathf.FloorToInt(0.025f * Mathf.Pow(i, 3) + 10f * Mathf.Pow(i, 2) + 100f * i + 500f);
        }
    }

    private int CalculateLevelCost(int currentLevel, int expectedLevel)
    {
        int totalCost = 0;
        for (int i = currentLevel; i < expectedLevel; i++)
        {
            if (i > playerLevels.Length - 1)
            {
                Debug.LogWarning("Level exceeds maximum defined levels. Returning crazy cost.");
                return int.MaxValue;
            }
            totalCost += playerLevels[i];
        }

        return totalCost;
    }

    private void ChangeTextColorsDependingOnCost()
    {
        PlayerManager player = PlayerUIManager.instance.playerManager;

        int expectedVigorLevel = Mathf.RoundToInt(vigorSlider.value);
        int expectedResistanceLevel = Mathf.RoundToInt(resistanceSlider.value);
        int expectedEnduranceLevel = Mathf.RoundToInt(enduranceSlider.value);
        int expectedStrengthLevel = Mathf.RoundToInt(strengthSlider.value);

        if (totalLevelUpCost > 0)
        {
            bloodDropsNeededText.color = Color.red;
        }
        else
        {
            bloodDropsNeededText.color = Color.white;
        }

        ChangeTextFieldToSpecificColorBasedOnValue(player, expectedVigorText, player.playerStatsManager.vitality, expectedVigorLevel);
        ChangeTextFieldToSpecificColorBasedOnValue(player, expectedResistanceText, player.playerStatsManager.resistance, expectedResistanceLevel);
        ChangeTextFieldToSpecificColorBasedOnValue(player, expectedEnduranceText, player.playerStatsManager.endurance, expectedEnduranceLevel);
        ChangeTextFieldToSpecificColorBasedOnValue(player, expectedStrengthText, player.playerStatsManager.strength, expectedStrengthLevel);

        if (totalLevelUpCost > player.playerStatsManager.bloodDrops)
        {
            expectedBloodDropsHeldText.color = Color.red;
        }
        else
        {
            expectedBloodDropsHeldText.color = Color.white;
        }
    }

    private void ChangeTextFieldToSpecificColorBasedOnValue(PlayerManager player, TextMeshProUGUI textField, int currentValue, int expectedValue)
    {
        if (currentValue == expectedValue)
        {
            textField.color = Color.white;
            return;
        }

        if (totalLevelUpCost <= player.playerStatsManager.bloodDrops)
        {
            if (expectedValue > currentValue)
            {
                textField.color = Color.green;
            }
            else
            {
                textField.color = Color.white;
            }
        }
        else
        {
            if (expectedValue > currentValue)
            {
                textField.color = Color.red;
            }
            else
            {
                textField.color = Color.white;
            }
        }
    }

}
