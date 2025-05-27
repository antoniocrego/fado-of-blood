using UnityEngine;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;
    [SerializeField] bool hasBeenDefeated = false;

    protected override void Awake()
    {
        base.Awake();

        // If boss was never put in the save data, he was never encountered, thus isnt awakened and much less defeated.
        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            // Initialize the boss as awakened if it doesn't exist in the dictionary.
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
        }
        else
        {
            hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];

            if (hasBeenDefeated)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
