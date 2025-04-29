using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerManager : CharacterManager
{
    //TODO: handles animations and stats
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

    [HideInInspector] public PlayerStatsManager playerStatsManager;

    [HideInInspector] public GameObject playerTarget; 

    [HideInInspector] public PlayerCamera playerCameraManager;

    public float lockOnRange = 20f;
    public LayerMask lockOnLayerMask;
    public float fieldOfView = 60f;
    protected override void Awake()
    {
        base.Awake();

        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
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

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        playerCameraManager.HandleCamera();
    }
}