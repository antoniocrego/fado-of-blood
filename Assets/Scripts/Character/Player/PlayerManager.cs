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

    [HideInInspector] public PlayerEffectsManager playerEffectsManager;
    public WeaponItem previousRightHandWeapon = null;

    public QuickSlotItem previousQuickSlotItem = null;

    public bool previousChuggingStatus = false;
    public bool previousIsBlockingStatus = false;
    public WeaponItem previousLeftHandWeapon = null;
    public WeaponItem currentWeaponBeingUsed;
    public bool isUsingRightHand = false;
    public bool isUsingLeftHand = false;
    public float lockOnRange = 20f;
    public float minFov = -60f;
    public float maxFov = 60f;

    private Coroutine revivalCoroutine;

    protected override void Start()
    {
        base.Start();

        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        if (playerCameraManager == null)
        {
            playerCameraManager = FindAnyObjectByType<PlayerCamera>();
        }
        if (WorldSaveGameManager.instance != null)
        {
            WorldSaveGameManager.instance.player = this;
        }

        maxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel();
        health = maxHealth;
        stamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel();
        maxStamina = Mathf.RoundToInt(stamina);

        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth);
        PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(maxHealth);
    }
    protected override void Update()
    {
        base.Update();

        PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(health);

        playerLocomotionManager.HandleAllMovement();

        // NEED A NEW WAY TO CHANGE STAMINA VALUE
        maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel();
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina);
        PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(stamina);
        if (playerInventoryManager.currentQuickSlotItem == null)
        {
            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(6);
        }
        if (playerInventoryManager.currentQuickSlotItem != previousQuickSlotItem)
        {
            QuickSlotItem newQuickSlotItem = null;
            int newID = playerInventoryManager.currentQuickSlotItem != null ? playerInventoryManager.currentQuickSlotItem.itemID : -1;

            QuickSlotItem quickSlotItemFromDB = WorldItemDatabase.Instance.GetQuickSlotItemByID(newID);
            if (quickSlotItemFromDB != null)
                newQuickSlotItem = Instantiate(quickSlotItemFromDB);

            if (newQuickSlotItem != null)
            {
                playerInventoryManager.currentQuickSlotItem = newQuickSlotItem;
                PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(newQuickSlotItem.itemID);
            }
        }
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

        if (previousChuggingStatus != playerEquipmentManager.isChugging)
        {
            animator.SetBool("isChuggingFlask", playerEquipmentManager.isChugging);
            previousChuggingStatus = playerEquipmentManager.isChugging;
        }

        if (previousIsBlockingStatus != isBlocking)
        {
            animator.SetBool("isBlocking", isBlocking);
            previousIsBlockingStatus = isBlocking;
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
        currentCharacterData.vitality = playerStatsManager.vitality;
        currentCharacterData.endurance = playerStatsManager.endurance;
        currentCharacterData.resistance = playerStatsManager.resistance;
        currentCharacterData.strength = playerStatsManager.strength;
        currentCharacterData.worldPositionX = transform.position.x;
        currentCharacterData.worldPositionY = transform.position.y;
        currentCharacterData.worldPositionZ = transform.position.z;
        currentCharacterData.bloodDrops = playerStatsManager.bloodDrops;
    }

    public void LoadGame(ref CharacterSaveData currentCharacterData)
    {
        playerName = currentCharacterData.characterName;

        playerStatsManager.vitality = currentCharacterData.vitality;
        playerStatsManager.endurance = currentCharacterData.endurance;
        playerStatsManager.resistance = currentCharacterData.resistance;
        playerStatsManager.strength = currentCharacterData.strength;

        if (currentCharacterData.isInForbiddenSaveArea)
        {
            transform.position = new Vector3(
                currentCharacterData.safeSavePositionX,
                currentCharacterData.safeSavePositionY,
                currentCharacterData.safeSavePositionZ
            );
        }
        else
        {
            // If not in a forbidden save area, use the saved world position
            transform.position = new Vector3(
                currentCharacterData.worldPositionX,
                currentCharacterData.worldPositionY,
                currentCharacterData.worldPositionZ
            );
        }

        playerStatsManager.bloodDrops = currentCharacterData.bloodDrops;

        if (currentCharacterData.hasBloodPool)
        {
            Vector3 bloodPoolPosition = new Vector3(
                currentCharacterData.bloodPoolPositionX,
                currentCharacterData.bloodPoolPositionY,
                currentCharacterData.bloodPoolPositionZ
            );
            CreateDeadSpot(bloodPoolPosition, currentCharacterData.bloodPoolBloodDrops, false);
        }

        if (playerStatsManager)
            PlayerUIManager.instance.playerUIHudManager.SetBloodDrops(playerStatsManager.bloodDrops);
    }

    public override IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
        WorldSoundFXManager.instance.PlayYouDiedSFX();
        CreateDeadSpot(transform.position, playerStatsManager.bloodDrops, true);

        yield return base.ProcessDeath(manuallySelectDeathAnimation);

        yield return new WaitForSeconds(6f);

        ReviveWrap();
    }

    public void CreateDeadSpot(Vector3 position, int bloodDropCount, bool generating = true)
    {
        GameObject bloodPoolVFX = Instantiate(
            WorldCharacterEffectsManager.instance.bloodPoolVFX,
            position,
            Quaternion.identity
        );
        BloodPoolInteractable bloodPoolInteractable = bloodPoolVFX.GetComponent<BloodPoolInteractable>();
        bloodPoolInteractable.bloodDrops = bloodDropCount;

        if (generating)
        {
            // player just died, we're creating a blood pool and adding it to the save data
            playerStatsManager.AddBloodDrops(-playerStatsManager.bloodDrops);

            WorldSaveGameManager.instance.currentCharacterData.hasBloodPool = true;
            WorldSaveGameManager.instance.currentCharacterData.bloodPoolPositionX = position.x;
            WorldSaveGameManager.instance.currentCharacterData.bloodPoolPositionY = position.y;
            WorldSaveGameManager.instance.currentCharacterData.bloodPoolPositionZ = position.z;
            WorldSaveGameManager.instance.currentCharacterData.bloodPoolBloodDrops = bloodDropCount;
        }
    }

    public void ReviveWrap()
    {
        if (revivalCoroutine != null)
        {
            StopCoroutine(revivalCoroutine);
        }
        revivalCoroutine = StartCoroutine(Revive());
    }

    private IEnumerator Revive()
    {
        LoadingScreenManager.instance.ActivateLoadingScreen();

        // let's end the Dying SFX early
        WorldSoundFXManager.instance.EndYouDiedSFXEarly();

        WorldAIManager.instance.ResetAllCharacters();

        CharacterSaveData currentCharacterData = WorldSaveGameManager.instance.currentCharacterData;

        int latestBonfireID = currentCharacterData.lastBonfireRestedAt;

        Vector3 respawnPosition = new Vector3(0f, 1.2f, 0f);

        if (latestBonfireID >= 0 && currentCharacterData.bonfiresLit.ContainsKey(latestBonfireID) && currentCharacterData.bonfiresLit[latestBonfireID])
        {
            respawnPosition = WorldObjectManager.instance.bonfires[latestBonfireID].respawnPosition.position;
        }
        else
        {
            Debug.LogWarning("Invalid bonfire ID, respawning at default position.");
        }

        RevivePlayer(respawnPosition);

        yield return null;
    }
    
    private void RevivePlayer(Vector3 respawnPosition)
    {
        health = maxHealth;
        stamina = maxStamina;

        transform.position = respawnPosition;

        PlayerUIManager.instance.CloseAllMenuWindows();
        PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

        playerCombatManager.currentTarget = null;
        playerCameraManager.ClearLockOnTargets();

        PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(maxHealth);
        PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(maxStamina);

        isDead = false;
        // there's conflicting "Empty" states so we have to specify the layer
        animator.Play("Empty", 2, 0f); // Reset animator state

        WorldSaveGameManager.instance.SaveGame();

        LoadingScreenManager.instance.DeactivateLoadingScreen();
    }

}