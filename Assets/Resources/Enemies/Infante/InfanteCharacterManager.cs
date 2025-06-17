using System.Collections;
using FMODUnity;
using UnityEngine;

public class InfanteCharacterManager : AIBossCharacterManager
{

    [Header("Infante Specific")]
    [SerializeField] string introSpeech = "event:/Voices/Final Boss/Final Boss Intro";
    [SerializeField] string deathSpeech = "event:/Voices/Final Boss/Final Boss Death";
    public override void WakeBoss()
    {
        if (hasBeenDefeated) return;

        if (!hasBeenAwakened)
        {
            RuntimeManager.PlayOneShot(introSpeech, transform.position);
        }

        characterAnimatorManager.PlayTargetActionAnimation(wakeAnimation, true);

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

    public override IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
    {
        // spawn game ending event triggers
        WorldSaveGameManager.instance.currentCharacterData.hasKilledTheFinalBoss = true;
        WorldObjectManager.instance.UpdateEndingTriggers();
        return base.ProcessDeath(manuallySelectDeathAnimation);
    }

    public override void ActivateBossFight()
    {
        base.ActivateBossFight();
        WorldSoundtrackManager.instance.PlayTrack("event:/Music/Final Boss Music", true);
    }

    public override void DeactivateBossFight()
    {
        base.DeactivateBossFight();
        WorldSoundtrackManager.instance.StopTrack();
    }

    public override void SoftDeactivateBossFight()
    {
        base.SoftDeactivateBossFight();
        WorldSoundtrackManager.instance.StopTrack();
    }
}
