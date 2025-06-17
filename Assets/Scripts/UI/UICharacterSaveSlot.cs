using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICharacterSaveSlot : MonoBehaviour
{
    SaveFileDataWriter saveFileDataWriter;

    [Header("Save Slot")]
    public CharacterSlot characterSlot;

    [Header("Character Info")]
    public TMP_Text characterName;
    public Image saveIcon;
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
                    saveIcon.color = GetColorFromEnding(WorldSaveGameManager.instance.characterSlot01);
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
                    saveIcon.color = GetColorFromEnding(WorldSaveGameManager.instance.characterSlot02);
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
                    saveIcon.color = GetColorFromEnding(WorldSaveGameManager.instance.characterSlot03);
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
                    saveIcon.color = GetColorFromEnding(WorldSaveGameManager.instance.characterSlot04);
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
                    saveIcon.color = GetColorFromEnding(WorldSaveGameManager.instance.characterSlot05);
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

    public Color GetColorFromEnding(CharacterSaveData characterData) {
        int endingID = characterData.endingID;
        Debug.Log($"Character Slot: {characterSlot}, Ending ID: {endingID}");
        switch (endingID)
        {
            case 0:
                return Color.white;
            case 1:
                return Color.blue;
            case 2:
                return Color.red;
            default:
                return Color.gray;
        }
    }
}
