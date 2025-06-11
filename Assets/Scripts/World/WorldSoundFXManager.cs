using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;

    [Header("Action Sounds")]
    public AudioClip rollSFX;

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

    public AudioClip ChooseRandomFootstepSoundBasedOnSurface(GameObject steppedOnObject, CharacterManager character)
    {
        if (steppedOnObject == null) return null;

        // TODO: ADD LOGIC FOR CHOOSING FOOTSTEP SOUND BASED ON SURFACE TYPE
        if (steppedOnObject.CompareTag("Grass"))
        {
            // Return a grass footstep sound
            return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepsGrass); // Assuming footstepSFXs is defined somewhere
        }
        else if (steppedOnObject.CompareTag("Stone"))
        {
            // Return a stone footstep sound
            return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepsStone); // Assuming footstepSFXs is defined somewhere
        }
        else if (steppedOnObject.CompareTag("Mud"))
        {
            // Return a mud footstep sound
            return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepsMud); // Assuming footstepSFXs is defined somewhere
        }

        return ChooseRandomSFXFromArray(character.characterSoundFXManager.footstepSFXs); // Default to the general footstep sounds
    }
}
