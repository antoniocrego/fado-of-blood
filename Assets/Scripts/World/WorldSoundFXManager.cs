using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;

    [Header("Action Sounds")]
    public AudioClip rollSFX;

    public AudioClip healingFlaskSFX;

    [Header("You Died SFX")]
    [SerializeField] protected string youDiedSFXFMODEvent = "event:/UI/You Died";
    [SerializeField] protected EventInstance youDiedInstance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);

        return array[index];
    }

    public void PlayYouDiedSFX()
    {
        youDiedInstance = RuntimeManager.CreateInstance(youDiedSFXFMODEvent);
        youDiedInstance.start();
    }

    public void EndYouDiedSFXEarly()
    {
        youDiedInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        youDiedInstance.release();
    }
}
