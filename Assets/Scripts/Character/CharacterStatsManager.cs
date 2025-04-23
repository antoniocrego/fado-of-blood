using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public int CalculateStaminaBasedOnEnduranceLevel(int endurance) 
    { 

        float   stamina = endurance * 10;

        return Mathf.RoundToInt(stamina); 
    }
}
