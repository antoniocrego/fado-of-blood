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
        base.WakeBoss();
        RuntimeManager.PlayOneShot(introSpeech, transform.position);
    }

    public override IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
    {
        // spawn game ending event triggers
        return base.ProcessDeath(manuallySelectDeathAnimation);
    }
}
