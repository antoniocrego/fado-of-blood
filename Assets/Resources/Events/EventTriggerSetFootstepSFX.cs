using UnityEngine;

public class EventTriggerSetFootstepSFX : MonoBehaviour
{
    [Header("Footstep SFX Settings")]
    [SerializeField] private string surfaceType = "Dirt";

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager character = other.GetComponent<CharacterManager>();
        if (character != null)
        {
            character.currentSurfaceType = surfaceType;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterManager character = other.GetComponent<CharacterManager>();
        if (character != null)
        {
            character.currentSurfaceType = "Dirt"; // Reset to default or another surface type
        }
    }
}