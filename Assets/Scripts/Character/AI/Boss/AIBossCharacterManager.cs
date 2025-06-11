using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;
    [SerializeField] bool hasBeenDefeated = false;
    [SerializeField] bool hasBeenAwakened = false;
    [SerializeField] List<FogWallInteractable> fogWalls;

    [Header("Debug")]
    [SerializeField] bool wakeBossUp = false;

    protected override void Start()
    {
        base.Start();

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
            hasBeenAwakened = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];
        }

        StartCoroutine(GetFogWallsFromWorldObjectManager());
    }

    private IEnumerator GetFogWallsFromWorldObjectManager()
    {
        while (WorldObjectManager.instance.fogWalls.Count == 0)
            yield return new WaitForEndOfFrame(); // Wait for the next frame to ensure the WorldObjectManager is initialized.

        fogWalls = new List<FogWallInteractable>();
        foreach (FogWallInteractable fogWall in WorldObjectManager.instance.fogWalls)
        {
            if (fogWall.fogWallID == bossID)
            {
                fogWalls.Add(fogWall);
            }
        }
        
        if (hasBeenAwakened)
        {
            foreach (FogWallInteractable fogWall in fogWalls)
            {
                fogWall.isActive = true;
            }
        }

        if (hasBeenDefeated)
        {
            isActive = false;
            foreach (FogWallInteractable fogWall in fogWalls)
            {
                fogWall.isActive = false;
            }
        }

    }

    public override IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
    {
        health = 0;
        isDead = true;

        // reset any needed flags

        // if we are not grounded play an aerial death animation

        if (!manuallySelectDeathAnimation)
        {
            // characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
        }

        hasBeenDefeated = true;

        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
        }
        else
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID] = true;
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID] = true;
        }

        // play death sfx

        yield return new WaitForSeconds(5);

        // award players with souls

        // disable character
    }

    public void WakeBoss()
    {
        hasBeenAwakened = true;
        // If boss was never put in the save data, he was never encountered, thus isnt awakened and much less defeated.
        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            // Initialize the boss as awakened if it doesn't exist in the dictionary.
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
        }
        else
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID] = true;
        }

        for (int i = 0; i < fogWalls.Count; i++)
        {
            fogWalls[i].isActive = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (wakeBossUp)
        {
            wakeBossUp = false;
            WakeBoss();
        }
    }
}
