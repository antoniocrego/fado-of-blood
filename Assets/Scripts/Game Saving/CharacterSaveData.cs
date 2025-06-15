using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName = "Lowly Husk";

    [Header("Time Played")]
    public float secondsPlayed = 0f;

    [Header("Character Stats")]
    public int vitality = 1;
    public int resistance = 1;
    public int endurance = 1;
    public int strength = 1;

    [Header("World Position")]
    public float worldPositionX = 0;
    public float worldPositionY = 0.81f;
    public float worldPositionZ = 0;

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



    public CharacterSaveData()
    {
        // Initialize the dictionaries to avoid null reference exceptions.
        worldItemsLooted = new SerializableDictionary<int, bool>();
        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();
        bonfiresLit = new SerializableDictionary<int, bool>();
    }
}
