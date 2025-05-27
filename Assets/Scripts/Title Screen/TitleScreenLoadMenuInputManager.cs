using UnityEngine;

public class TitleScreenLoadMenuInputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Title Screen Inputs")]
    [SerializeField] bool deleteCharacterSlot = false;

    private void Update()
    {
        if (deleteCharacterSlot)
        {
            deleteCharacterSlot = false;
            TitleScreenManager.instance.AttemptToDeleteCharacterSlot();
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }
        playerControls.Enable();
        playerControls.UI.X.performed += ctx => deleteCharacterSlot = true;
    }
    
    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
            playerControls.UI.X.performed -= ctx => deleteCharacterSlot = true;
        }
    }
}
