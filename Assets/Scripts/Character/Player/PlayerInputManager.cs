using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public PlayerManager player;

    public float verticalInput;

    public float horizontalInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    public float movementCombined; 
    PlayerControls playerControls;

    [Header("Player Action Input")]
    [SerializeField] Vector2 movementInput;
    [SerializeField] Vector2 cameraInput;
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool lockedOn_input = false;
    [SerializeField] bool RB_Input = false;
    [SerializeField] bool lockOnLeft_input = false;
    [SerializeField] bool lockOnRight_input = false;

    [SerializeField] bool switch_Right_hand_weapon = false;
    [SerializeField] bool switch_Left_hand_weapon = false;

    [SerializeField] bool interaction_input = false;

    [SerializeField] bool use_Item_input = false;


    
    [SerializeField] bool openCharacterMenuInput = false;
    [SerializeField] bool closeMenuInput = false;

    private Coroutine lockOnCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (player == null)
            {
                player = FindAnyObjectByType<PlayerManager>();
            }
        }
        else
        {
            Destroy(gameObject);

        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        SceneManager.activeSceneChanged += OnSceneChange;   
        
        instance.enabled = true;
    }

    private void Update() 
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandleUseItemInput();
        HandleMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleLockOnInput();
        HandleRBInput();
        HandleLockOnSwitchTargetInput();
        HandleCameraInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
        HandleInteractInput();
        HandleCloseUIInput();  
        HandleOpenCharacterMenuInput();
    }


    private void HandleMovementInput() 
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x; 


        movementCombined = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if(movementCombined <= 0.5 && movementCombined > 0) 
        {
            movementCombined = 0.5f;
        }
        else if(movementCombined > 0.5 && movementCombined < 1) 
        {
            movementCombined = 1f;
        }
        if(player == null) 
        {
            return;
        }
        if(movementCombined!=0){
            player.isMoving = true;
        }
        else{
            player.isMoving = false;
        }
        if (!player.playerLocomotionManager.canRun)
        {
            if (movementCombined > 0.5f)
            {
                movementCombined = 0.5f;
            }

            if (verticalInput > 0.5f)
            {
                verticalInput = 0.5f;
            }

            if (horizontalInput > 0.5f)
            {
                horizontalInput = 0.5f;
            }
        }
        if (!lockedOn_input || player.isSprinting)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, movementCombined, player.isMoving, player.isSprinting);
        }
        else if (lockedOn_input)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.isMoving, player.isSprinting);
        }
    }

    private void HandleDodgeInput() 
    {
        if(dodgeInput) 
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }
    
    private void HandleInteractInput() 
    {
        if(interaction_input) 
        {
            interaction_input = false;
            player.playerInteractionManager.Interact();
        }
    }

   

    private void HandleSprintInput()
    {
        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSprint();
        }
        else
        {
            player.isSprinting = false;
        }
    }

    private void HandleLockOnInput() 
    {
        if (player.isLockedOn)
        {
            if (player.playerCombatManager.currentTarget == null)
            {
                return;
            }
            if (player.playerCombatManager.currentTarget.isDead)
            {
                player.isLockedOn = false;
                if (lockOnCoroutine != null)
                {
                    StopCoroutine(lockOnCoroutine);
                }
                lockOnCoroutine = StartCoroutine(player.playerCameraManager.WaitThenFindNewTarget());
            }

        }
        if(lockedOn_input && player.isLockedOn)
        {
            lockedOn_input = false;
            player.playerCameraManager.ClearLockOnTargets();
            player.playerCombatManager.currentTarget = null;
            player.isLockedOn = false;
            //Disable the lock on
            return;
        }

        if(lockedOn_input && !player.isLockedOn)
        {
            lockedOn_input = false;
            player.playerCameraManager.HandleLocatingLockOnTargets();
            if(player.playerCameraManager.currentLockOnTarget != null)
            {
                player.isLockedOn = true;
                player.playerCombatManager.SetTarget(player.playerCameraManager.currentLockOnTarget);
            }
                
            return;
        }
    }

    private void HandleRBInput()
    {
        if(RB_Input)
        {
            RB_Input = false;

            // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

            player.SetCharacterActionHand(true);

            // TODO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleLockOnSwitchTargetInput()
    {
        if (lockOnLeft_input)
        {
            lockOnLeft_input = false;
            if (player.isLockedOn)
            {
                player.playerCameraManager.HandleLocatingLockOnTargets();

                if (player.playerCameraManager.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(player.playerCameraManager.leftLockOnTarget);
                }
            }
        }
        
        if (lockOnRight_input)
        {
            lockOnRight_input = false;
            if (player.isLockedOn)
            {
                player.playerCameraManager.HandleLocatingLockOnTargets();

                if (player.playerCameraManager.rightLockOnTarget != null)
                {   
                    player.playerCombatManager.SetTarget(player.playerCameraManager.rightLockOnTarget);
                }
            }
        }
    }

    private void HandleCameraInput() 
    {
        if(player == null) 
        {
            return;
        }

        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x; 
    }
    private void OnSceneChange(Scene current, Scene next)
    {
        // TODO: CHANGE THIS TO USE SCENE NUMBERS DEFINED ELSEWHERE
        if (next.buildIndex == 0){
            instance.enabled = false;
        }
        else{
            instance.enabled = true;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.RB.performed += instance => RB_Input = true;
            playerControls.PlayerActions.LockOn.performed += i => lockedOn_input = true;
            playerControls.PlayerActions.LockOnLeft.performed += i => lockOnLeft_input = true;
            playerControls.PlayerActions.LockOnRight.performed += i => lockOnRight_input = true;
            playerControls.PlayerActions.Interact.performed += i => interaction_input = true;

            playerControls.PlayerActions.X.performed += i => use_Item_input = true;

            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            playerControls.CameraMovement.Look.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_hand_weapon = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_hand_weapon = true;

            playerControls.PlayerActions.Dodge.performed += i => closeMenuInput = true;
            playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;


        }

        playerControls.Enable();
    }
    
    private void HandleUseItemInput()
    {
        if (use_Item_input)
        {
            use_Item_input = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;

            if (player.playerInventoryManager.currentQuickSlotItem != null)
            {
                player.playerInventoryManager.currentQuickSlotItem.AttemptToUseItem(player);
                player.playerEquipmentManager.QuickSlotItemUse(player.playerInventoryManager.currentQuickSlotItem.itemID);
            }
        }
    }

    private void HandleSwitchRightWeaponInput()
    {
        if (switch_Right_hand_weapon)
        {
            switch_Right_hand_weapon = false;
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftWeaponInput()
    {
        if(switch_Left_hand_weapon)
        {
            switch_Left_hand_weapon = false;
            if (PlayerUIManager.instance.menuWindowIsOpen)
                return; 
            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }

    private void HandleOpenCharacterMenuInput()
    {
        if (openCharacterMenuInput)
        {
            openCharacterMenuInput = false;

            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            PlayerUIManager.instance.CloseAllMenuWindows();
            PlayerUIManager.instance.playerUICharacterMenuManager.OpenCharacterMenu();
        }
    }

    private void HandleCloseUIInput()
    {
        if (closeMenuInput)
        {
            closeMenuInput = false;

            if (PlayerUIManager.instance.menuWindowIsOpen)
            {
                PlayerUIManager.instance.CloseAllMenuWindows();
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
