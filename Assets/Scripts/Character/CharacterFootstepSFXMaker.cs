using UnityEngine;

public class CharacterFootstepSFXMaker : MonoBehaviour
{
    CharacterManager character;

    AudioSource audioSource;
    GameObject steppedOnObject;

    private bool hasTouchedGround = false;
    private bool hasPlayedFootstepSFX = false;
    [SerializeField] float distanceToGround = 0.05f; // Distance to check for ground

    private void Start()
    {
        character = GetComponentInParent<CharacterManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {

    }

    private void CheckForFootSteps()
    {
        if (character == null) return;

        if (!character.isMoving) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.Instance.GetEnviroLayer()))
        {
            hasTouchedGround = true;

            if (!hasPlayedFootstepSFX)
            {
                steppedOnObject = hit.transform.gameObject;
            }
        }
        else
        {
            hasTouchedGround = false;
            hasPlayedFootstepSFX = false;
            steppedOnObject = null;
        }

        if (hasTouchedGround && !hasPlayedFootstepSFX)
        {
            hasPlayedFootstepSFX = true;
            PlayFootstepSFX();
        }
    }

    private void PlayFootstepSFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomFootstepSoundBasedOnSurface(steppedOnObject, character), 0.5f);
    }
}
