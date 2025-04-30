using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool switchRightWeapon = false;
    [SerializeField] bool switchLeftWeapon = false;

    //TODO: handles animations and stats
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public GameObject playerTarget; 
    [HideInInspector] public PlayerCamera playerCameraManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;

    public WeaponItem currentWeaponBeingUsed;
    public bool isUsingRightHand = false;
    public bool isUsingLeftHand = false;

    protected override void Awake()
    {
        base.Awake();

        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerCameraManager = GetComponent<PlayerCamera>();
        playerCombatManager = GetComponent<PlayerCombatManager>();

    }

    protected override void Update()
    {
        base.Update();

        playerLocomotionManager.HandleAllMovement();

        // NEED A NEW WAY TO CHANGE STAMINA VALUE
        maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(endurance);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina); 
        PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(stamina);

        DebugMenu();
    }

    private void DebugMenu(){
        if(switchRightWeapon){
            switchRightWeapon = false;
            playerEquipmentManager.SwitchRightWeapon();
        }

        if(switchLeftWeapon){
            switchLeftWeapon = false;
            playerEquipmentManager.SwitchLeftWeapon();
        }
    }

    public void SetCharacterActionHand(bool rightHandedAction)
    {
        if(rightHandedAction)
        {
            isUsingLeftHand = false;
            isUsingRightHand = true;
        }
        else
        {
            isUsingLeftHand = true;
            isUsingRightHand = false;
        }
    }
}