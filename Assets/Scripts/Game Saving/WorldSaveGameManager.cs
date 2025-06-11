using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public PlayerManager player;

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
        LoadAllCharacterSlots();
    }

    public string DecideCharacterFileName(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
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

        return fileName;
    }

    public void AttemptToCreateNewGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot01);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlot = CharacterSlot.CharacterSlot01;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot02);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlot = CharacterSlot.CharacterSlot02;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot03);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlot = CharacterSlot.CharacterSlot03;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot04);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlot = CharacterSlot.CharacterSlot04;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot05);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlot = CharacterSlot.CharacterSlot05;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        TitleScreenManager.instance.DisplayNoFreeCharactersPopUp();
    }

    public void LoadGame()
    {
        fileName = DecideCharacterFileName(currentCharacterSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        fileName = DecideCharacterFileName(currentCharacterSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;

        player.SaveGame(ref currentCharacterData);

        saveFileDataWriter.CreateNewSaveFile(currentCharacterData);
    }

    public void DeleteGame(CharacterSlot characterSlot)
    {
        fileName = DecideCharacterFileName(characterSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;
        saveFileDataWriter.DeleteSaveFile();
    }

    private void LoadAllCharacterSlots()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileName(CharacterSlot.CharacterSlot05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        player.LoadGame(ref currentCharacterData);
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadGame();
        }   
    }
}
