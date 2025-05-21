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
    public float lockOnRange = 20f;
    public float minFov = -60f;
    public float maxFov = 60f;

    public void Start()
    {
        maxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(vitality);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(100);
        PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(100);   
    }
    protected override void Awake()
    {
        base.Awake();

        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        if (playerCameraManager == null)
        {
            playerCameraManager = FindAnyObjectByType<PlayerCamera>();
        }
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

    protected override void LateUpdate()
    {
        base.LateUpdate();

        playerCameraManager.HandleCamera();
    }
}