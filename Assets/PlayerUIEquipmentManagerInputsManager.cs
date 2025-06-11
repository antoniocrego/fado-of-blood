using UnityEngine;

public class PlayerUIEquipmentManagerInputsManager : MonoBehaviour
{
    PlayerControls playerControls;

    PlayerUIEquipmentManager playerUIEquipmentManager;

    [SerializeField] bool unequipItemInput;

    private void Awake()
    {
        playerUIEquipmentManager = GetComponentInParent<PlayerUIEquipmentManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerActions.X.performed += i => unequipItemInput = true;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandlePlayerUIEquipmentManagerInputs();
    }

    private void HandlePlayerUIEquipmentManagerInputs()
    {
        if (unequipItemInput)
        {
            unequipItemInput = false; 
            playerUIEquipmentManager.UnEquipSelectedItem();
        }
    }
    
}
