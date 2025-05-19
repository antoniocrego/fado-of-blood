using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Position")]
    public float worldPositionX;
    public float worldPositionY;
    public float worldPositionZ;
}
