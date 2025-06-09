using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerManager : CharacterManager
{
    [Header("Player Name")]
    public string playerName = "Player";

    //TODO: handles animations and stats
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public GameObject playerTarget;
    [HideInInspector] public PlayerCamera playerCameraManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;

    [HideInInspector] public PlayerInteractionManager playerInteractionManager;

    public WeaponItem previousRightHandWeapon = null;

    public WeaponItem previousLeftHandWeapon = null;
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
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        if (playerCameraManager == null)
        {
            playerCameraManager = FindAnyObjectByType<PlayerCamera>();
        }
        if (WorldSaveGameManager.instance != null)
        {
            WorldSaveGameManager.instance.player = this;
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

        if (playerInventoryManager.currentRightHandWeapon != previousRightHandWeapon)
        {
            WeaponItem newWeapon = playerInventoryManager.currentRightHandWeapon;
            playerInventoryManager.currentRightHandWeapon = newWeapon;
            playerEquipmentManager.LoadRightWeapon();
            playerUIHudManager.SetRightWeaponQuickSlotIcon(newWeapon.itemID);
        }

        if (playerInventoryManager.currentLeftHandWeapon != previousLeftHandWeapon)
        {
            WeaponItem newWeapon = playerInventoryManager.currentLeftHandWeapon;
            playerInventoryManager.currentLeftHandWeapon = newWeapon;
            playerEquipmentManager.LoadLeftWeapon();
            playerUIHudManager.SetLeftWeaponQuickSlotIcon(newWeapon.itemID);
        }
        
        previousRightHandWeapon = playerInventoryManager.currentRightHandWeapon;
        previousLeftHandWeapon = playerInventoryManager.currentLeftHandWeapon;

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