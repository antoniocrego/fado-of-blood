using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    [Header("Attack Whooshes")]
    [SerializeField] protected AudioClip[] attackWhooshes;

    [Header("Death SFX")]
    [SerializeField] protected AudioClip deathSFX;

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
}
