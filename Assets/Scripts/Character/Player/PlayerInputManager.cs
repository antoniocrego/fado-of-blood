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

    [SerializeField] Vector2 movementInput;

    [SerializeField] bool dodgeInput = false;

    [SerializeField] bool sprintInput = false;

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
        Debug.Log("Player is ", player);
        if(player == null) 
        {
            return;
        }
        player.playerAnimatorManager.updateAnimatorMovementParameters(0, movementCombined, player.isSprinting);
    }

    private void HandleDodgeInput() 
    {
        if(dodgeInput) 
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
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
