using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    // 90% sure i'll have to ask the player to assign itself
    private PlayerManager player;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    public SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlot;
    public CharacterSaveData currentCharacterData;
    private string fileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void DecideCharacterFileName()
    {
        switch (currentCharacterSlot)
        {
            case CharacterSlot.CharacterSlot01:
                fileName = "CharacterSlot01";
                break;
            case CharacterSlot.CharacterSlot02:
                fileName = "CharacterSlot02";
                break;
            case CharacterSlot.CharacterSlot03:
                fileName = "CharacterSlot03";
                break;
            case CharacterSlot.CharacterSlot04:
                fileName = "CharacterSlot04";
                break;
            case CharacterSlot.CharacterSlot05:
                fileName = "CharacterSlot05";
                break;
        }
    }

    public void NewGame()
    {
        DecideCharacterFileName();

        currentCharacterData = new CharacterSaveData();
    }

    public void LoadPreviousGame()
    {
        DecideCharacterFileName();

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        DecideCharacterFileName();

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;

        player.SaveGame(ref currentCharacterData);

        saveFileDataWriter.CreateNewSaveFile(currentCharacterData);
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
