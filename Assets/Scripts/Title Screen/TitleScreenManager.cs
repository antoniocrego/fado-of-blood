using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;
    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button loadMenuReturnButton;

    [Header("PopUps")]
    [SerializeField] GameObject noFreeCharactersPopUp;
    [SerializeField] Button noFreeCharactersPopUpOKButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;
    [SerializeField] Button deleteCharacterSlotPopUpConfirmButton;
    [SerializeField] Button deleteCharacterSlotPopUpCancelButton;

    [Header("Character Slots")]
    public CharacterSlot currentCharacterSlot = CharacterSlot.NoSlot;


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

    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();
    }

    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenLoadMenu.SetActive(false);
        titleScreenMainMenu.SetActive(true);

        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharactersPopUp()
    {
        noFreeCharactersPopUp.SetActive(true);

        noFreeCharactersPopUpOKButton.Select();
    }

    public void CloseNoFreeCharactersPopUp()
    {
        noFreeCharactersPopUp.SetActive(false);

        mainMenuNewGameButton.Select();
    }

    // Character Slots

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentCharacterSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentCharacterSlot = CharacterSlot.NoSlot;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if (currentCharacterSlot != CharacterSlot.NoSlot)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deleteCharacterSlotPopUpConfirmButton.Select();
        }
    }

    public void DeleteCharacterSlot()
    {
        WorldSaveGameManager.instance.DeleteGame(currentCharacterSlot);

        // Refresh the load menu
        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        CloseDeleteCharacterSlotPopUp();
    }

    public void CloseDeleteCharacterSlotPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }
}
