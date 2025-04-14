using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterInventoryManager inventoryManager;
    public CharacterEquipmentManager equipmentManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    public float health;
    public float stamina;
    public bool isDead = false;

    protected virtual void Awake()
    {
        inventoryManager = GetComponent<CharacterInventoryManager>();
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        health = 100;
        stamina = 100;
    }
}