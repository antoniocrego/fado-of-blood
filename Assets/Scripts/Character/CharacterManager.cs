using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterInventoryManager inventoryManager;
    public CharacterEquipmentManager equipmentManager;
    // public CharacterStatsManager statsManager;
    // public CharacterAnimatorManager animatorManager;
    // public CharacterEffectsManager effectsManager;
    // public CharacterAudioManager audioManager;

    protected virtual void Awake()
    {
        inventoryManager = GetComponent<CharacterInventoryManager>();
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        // statsManager = GetComponent<CharacterStatsManager>();
        // animatorManager = GetComponent<CharacterAnimatorManager>();
        // effectsManager = GetComponent<CharacterEffectsManager>();
        // audioManager = GetComponent<CharacterAudioManager>();
    }
}