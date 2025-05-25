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
}
