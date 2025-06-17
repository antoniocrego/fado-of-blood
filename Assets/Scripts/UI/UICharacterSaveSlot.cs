using UnityEngine;
using TMPro;

public class UICharacterSaveSlot : MonoBehaviour
{
    SaveFileDataWriter saveFileDataWriter;

    [Header("Save Slot")]
    public CharacterSlot characterSlot;

    [Header("Character Info")]
    public TMP_Text characterName;
    public TMP_Text timePlayed;

    private void OnEnable()
    {
        LoadSaveSlot();
    }

    private void LoadSaveSlot()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileName(characterSlot);
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot01:
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                    timePlayed.text = TimeUtility.FormatTime(WorldSaveGameManager.instance.characterSlot01.secondsPlayed);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot02:
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                    timePlayed.text = TimeUtility.FormatTime(WorldSaveGameManager.instance.characterSlot02.secondsPlayed);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot03:
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                    timePlayed.text = TimeUtility.FormatTime(WorldSaveGameManager.instance.characterSlot03.secondsPlayed);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot04:
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                    timePlayed.text = TimeUtility.FormatTime(WorldSaveGameManager.instance.characterSlot04.secondsPlayed);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot05:
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                    timePlayed.text = TimeUtility.FormatTime(WorldSaveGameManager.instance.characterSlot05.secondsPlayed);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    public void LoadGameFromCharacterSlot()
    {
        WorldSaveGameManager.instance.currentCharacterSlot = characterSlot;
        WorldSaveGameManager.instance.LoadGame();
    }

    public void SelectCurrentSlot()
    {
        TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
    }
}
