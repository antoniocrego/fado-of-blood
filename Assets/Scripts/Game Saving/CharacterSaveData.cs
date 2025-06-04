using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName = "Lowly Husk";

    [Header("Time Played")]
    public float secondsPlayed = 0f;

    [Header("World Position")]
    public float worldPositionX = 0;
    public float worldPositionY = 0;
    public float worldPositionZ = 0;

    [Header("Boss Statuses")]
    public SerializableDictionary<int, bool> bossesAwakened;
    public SerializableDictionary<int, bool> bossesDefeated;
    
    [Header("World Items")]
    public SerializableDictionary<int, bool> worldItemsLooted; 

    public CharacterSaveData()
    {
        // Initialize the dictionaries to avoid null reference exceptions.
        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();
    }
}
