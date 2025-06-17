using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName = "Lowly Husk";

    [Header("Time Played")]
    public float secondsPlayed = 0f;

    [Header("Blood Pool")]
    public bool hasBloodPool = false;
    public float bloodPoolPositionX;
    public float bloodPoolPositionY;
    public float bloodPoolPositionZ;
    public int bloodPoolBloodDrops;

    [Header("Character Stats")]
    public int vitality = 1;
    public int resistance = 1;
    public int endurance = 1;
    public int strength = 1;

    [Header("World Position")]
    public float worldPositionX = 0;
    public float worldPositionY = 1.2f;
    public float worldPositionZ = 0;

    // if a player saves inside a boss area
    [Header("Safe Save Position")]
    public bool isInForbiddenSaveArea = false; // Indicates if the player is in a forbidden save area
    public float safeSavePositionX = 0f;
    public float safeSavePositionY = 1.2f;
    public float safeSavePositionZ = 0f;

    [Header("Current Area")]
    public int currentAreaID = 0; // Default to the first area

    [Header("Boss Statuses")]
    public SerializableDictionary<int, bool> bossesAwakened;
    public SerializableDictionary<int, bool> bossesDefeated;
    
    [Header("World Items")]
    public SerializableDictionary<int, bool> worldItemsLooted;

    [Header("Bonfires")]
    public SerializableDictionary<int, bool> bonfiresLit;
    public int lastBonfireRestedAt = -1;

    [Header("Blood Drops")]
    public int bloodDrops = 0;

    [Header("Levers")]
    public SerializableDictionary<int, bool> leversPulled;

    [Header("Doors")]
    public SerializableDictionary<int, bool> doorsOpened;

    [Header("Equipped Weapons")]
    public int rightWeaponIndex;
    public SerializableWeapon rightWeapon01;
    public SerializableWeapon rightWeapon02;
    public SerializableWeapon rightWeapon03;

    public int leftWeaponIndex;
    public SerializableWeapon leftWeapon01;
    public SerializableWeapon leftWeapon02;
    public SerializableWeapon leftWeapon03;

    public SerializableQuickSlotItem quickSlotItem01;
    
    public int remainingHealthFlasks = 3;

    [Header("Inventory")]
    public List<SerializableWeapon> weaponsInInventory;
    public List<SerializableQuickSlotItem> quickSlotItemsInInventory;
    public List<SerializableItem> otherItemsInInventory;

    public CharacterSaveData()
    {
        // Initialize the dictionaries to avoid null reference exceptions.
        worldItemsLooted = new SerializableDictionary<int, bool>();
        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();
        bonfiresLit = new SerializableDictionary<int, bool>();
        leversPulled = new SerializableDictionary<int, bool>();
        doorsOpened = new SerializableDictionary<int, bool>();
        weaponsInInventory = new List<SerializableWeapon>();
        quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
    }
}
