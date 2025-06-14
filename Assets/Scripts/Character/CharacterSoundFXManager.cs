using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected string damageGruntFMODEvent = "event:/Enemies/Small_Enemies/Moan";

    [Header("Damage SFX")]
    [SerializeField] protected string damageSFXFMODEvent = "event:/Combat/Blood and gore/Bone Crack";

    [Header("Attack Grunts")]
    [SerializeField] protected string attackGruntFMODEvent = ""; // no default grunts right now

    [Header("Attack Whooshes")]
    [SerializeField] protected AudioClip[] attackWhooshes;

    [Header("Death SFX")]
    [SerializeField] protected string deathSFXFMODEvent = "event:/Enemies/Small_Enemies/Death";

    [Header("Footstep SFX")]
    [SerializeField] public string footstepFMODEvent = "event:/Footsteps/Footsteps";

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);
        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
    }

    public void PlayDeathSFX()
    {
        RuntimeManager.PlayOneShot(deathSFXFMODEvent, transform.position);
    }

    public void PlayDamageSFX()
    {
        RuntimeManager.PlayOneShot(damageGruntFMODEvent, transform.position);
        RuntimeManager.PlayOneShot(damageSFXFMODEvent, transform.position);
    }

    public void PlayAttackGruntSFX()
    {
        if (!string.IsNullOrEmpty(attackGruntFMODEvent))
        {
            RuntimeManager.PlayOneShot(attackGruntFMODEvent, transform.position);
        }
    }
}
