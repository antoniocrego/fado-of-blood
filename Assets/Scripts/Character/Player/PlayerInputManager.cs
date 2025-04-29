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

    [SerializeField] Vector2 movementInput;
    [SerializeField] Vector2 cameraInput;

    [SerializeField] bool dodgeInput = false;

    [SerializeField] bool sprintInput = false;

    [SerializeField] bool jumpInput = false;

    [SerializeField] bool lockedOn_input = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if(player == null) 
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
        HandleMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleLockOnInput();
        HandleCameraInput();
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

    private void HandleLockOnTargets() 
    {
        GameObject target = player.playerCameraManager.FindLockOnTarget();
        if(target != null) 
        {
            player.playerTarget = target;
            player.isLockedOn = true;
            PlayerCamera.instance.isCameraLocked = true;
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
            lockedOn_input = false;
            
            if(player.isLockedOn)
            {
                ClearLockOnTargets();
                return;
            }
            Debug.Log("Attempting to lock on to target");

            HandleLockOnTargets();
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
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.LockOn.performed += i => lockedOn_input = true;


            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            playerControls.CameraMovement.Look.performed += i => cameraInput = i.ReadValue<Vector2>();

        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
