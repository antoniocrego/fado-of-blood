using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;

    [Header("Status")]
    public bool bossFightIsActive = false;
    [SerializeField] bool hasBeenDefeated = false;
    [SerializeField] bool hasBeenAwakened = false;
    [SerializeField] List<FogWallInteractable> fogWalls;
    [SerializeField] string sleepAnimation;
    [SerializeField] string wakeAnimation;

    [Header("States")]
    [SerializeField] AIState sleepState;

    [Header("Defeat Message")]
    [SerializeField] string defeatMessage = "GREAT FOE DEFEATED";

    protected override void Start()
    {
        base.Start();
        sleepState = Instantiate(sleepState);

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

        if (!hasBeenAwakened)
        {
            currentState = sleepState; // Set the initial state to sleep.
            characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
        }
    }

    public override void SetToInitialState()
    {
        currentState = sleepState; // Reset to sleep state.
        characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
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

        PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp(defeatMessage);
        characterSoundFXManager.PlayBossDefeatedSFX();

        // reset any needed flags
        DeactivateBossFight();

        // if we are not grounded play an aerial death animation

        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
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
        characterSoundFXManager.PlayDeathSFX();

        yield return new WaitForSeconds(3f);

        PlayerManager player = FindFirstObjectByType<PlayerManager>();

        if (player != null)
        {
            player.playerStatsManager.AddBloodDrops(aiCharacterStatsManager.bloodDroppedOnDeath);
        }

        // disable character
    }

    public virtual void WakeBoss()
    {
        if (!hasBeenAwakened)
        {
            characterAnimatorManager.PlayTargetActionAnimation(wakeAnimation, true);
        }

        ActivateBossFight();
        hasBeenAwakened = true;
        currentState = idleState;
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

        WorldSaveGameManager.instance.SaveGame();
    }

    public void ActivateBossFight()
    {
        if (bossFightIsActive) return;
        bossFightIsActive = true;
        GameObject bossHealthBar = Instantiate(PlayerUIManager.instance.playerUIHudManager.bossHealthBarPrefab, PlayerUIManager.instance.playerUIHudManager.bossHealthBarParent);

        UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
        bossHPBar.EnableBossHPBar(this);
    }

    public void DeactivateBossFight()
    {
        bossFightIsActive = false;
        // hp bar kills itself after 2.5 seconds

        characterLocomotionManager.canMove = false;
        characterLocomotionManager.canRotate = false;
        isPerformingAction = false;
        isSprinting = false;
        isMoving = false;

        foreach (FogWallInteractable fogWall in fogWalls)
        {
            fogWall.isActive = false;
        }
    }
}
