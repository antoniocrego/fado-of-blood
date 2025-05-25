using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerManager : CharacterManager
{
    [Header("Player Name")]
    public string playerName = "Player";

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
    public LayerMask lockOnLayerMask;
    public float fieldOfView = 60f;
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
        WorldSaveGameManager.instance.player = this;

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

    private void DebugMenu()
    {
        if (switchRightWeapon)
        {
            switchRightWeapon = false;
            playerEquipmentManager.SwitchRightWeapon();
        }

        if (switchLeftWeapon)
        {
            switchLeftWeapon = false;
            playerEquipmentManager.SwitchLeftWeapon();
        }
    }

    public void SetCharacterActionHand(bool rightHandedAction)
    {
        if (rightHandedAction)
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

    public void SaveGame(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.characterName = playerName;
        currentCharacterData.worldPositionX = transform.position.x;
        currentCharacterData.worldPositionY = transform.position.y;
        currentCharacterData.worldPositionZ = transform.position.z;
    }
    
    public void LoadGame(ref CharacterSaveData currentCharacterData)
    {
        playerName = currentCharacterData.characterName;
        transform.position = new Vector3(
            currentCharacterData.worldPositionX,
            currentCharacterData.worldPositionY,
            currentCharacterData.worldPositionZ
        );
    }

}