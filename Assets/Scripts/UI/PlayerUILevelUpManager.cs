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
            totalCost += playerLevels[i];
        }

        return totalCost;
    }

}
