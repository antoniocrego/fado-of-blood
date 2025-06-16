using UnityEngine;
using FMODUnity;
using Unity.VisualScripting;

public class CharacterFootstepSFXMaker : MonoBehaviour
{
    CharacterManager character;

    GameObject steppedOnObject;

    private bool hasTouchedGround = false;
    private bool hasPlayedFootstepSFX = false;
    [SerializeField] float distanceToGround = 0.1f; // Distance to check for ground
    [SerializeField] string footstepSFXEvent = "event:/Footsteps/Footsteps"; // FMOD event for footsteps

    private void Start()
    {
        character = GetComponentInParent<CharacterManager>();
    }

    private void FixedUpdate()
    {
        if (character == null) return;

        CheckForFootSteps();
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
        FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(footstepSFXEvent);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        // TODO: Add logic to choose the correct footstep sound based on the surface type

        instance.setParameterByNameWithLabel("Footsteps", character.currentSurfaceType);

        instance.start();
        instance.release();
    }
}
