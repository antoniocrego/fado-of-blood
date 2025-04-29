using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public PlayerManager player;

    public float verticalInput;

    public float horizontalInput;

    public float movementCombined; 
    PlayerControls playerControls;

    [Header("Player Action Input")]
    [SerializeField] Vector2 movementInput;
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool lockedOn_input = false;
    [SerializeField] bool RB_Input = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if(player == null) 
            {
                player = FindObjectOfType<PlayerManager>();
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
        HandleMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleLockOnInput();
        HandleRBInput();
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
        if(!lockedOn_input) 
        {
            player.playerAnimatorManager.updateAnimatorMovementParameters(0, movementCombined, player.isSprinting);
        }
        else if(lockedOn_input)
        {
            player.playerAnimatorManager.updateAnimatorMovementParameters(horizontalInput, verticalInput, player.isSprinting);
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

    private void HandleJumpInput() 
    {
        if(jumpInput) 
        {
            jumpInput = false; 

            player.playerLocomotionManager.AttemptToPerformJump();
            
        }
    }

    private void HandleSprintInput() 
    {
        if(sprintInput) 
        {
            player.playerLocomotionManager.HandleSprint(); 
        }
        else 
        {
            player.isSprinting = false;
        }
    }

    private void HandleLockOnTarget() 
    {
        // Define the target of the camera to be the boss
        GameObject target = GameObject.FindGameObjectWithTag("Boss");
        if(target != null) 
        {
            player.isLockedOn = true;
            PlayerCamera.instance.isCameraLocked = true;
            player.playerTarget = target; 
        }
        else 
        {
            ClearLockOnTargets();
        }

    }

    private void ClearLockOnTargets() 
    {
        // Clear the lock on targets
        player.isLockedOn = false;
        lockedOn_input = false;
        player.playerTarget = null;
        PlayerCamera.instance.isCameraLocked = false;
    }

    private void HandleLockOnInput() 
    {
        if(lockedOn_input)
        {
            if(GameObject.FindGameObjectWithTag("Boss") != null) 
            {
                HandleLockOnTarget();
            }
            else 
            {
                ClearLockOnTargets();
            }
        }
        else 
        {
            if(player.isLockedOn) 
            {
                ClearLockOnTargets();
            }
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
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.LockOn.performed += i => lockedOn_input = !lockedOn_input;
            playerControls.PlayerActions.RB.performed += instance => RB_Input = true;


            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
