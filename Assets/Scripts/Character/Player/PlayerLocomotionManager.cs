using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public PlayerManager player;
    public float verticalMovement; 

    public float horizontalMovement;

    public float movementCombined;

    private Vector3 movementDirection;

    private Vector3 targetDirection;

    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float runningSpeed = 5f; 

    [SerializeField] private float walkingSpeed = 2f;

    [SerializeField] private float sprintSpeed = 7f;

    private Vector3 rollDirection;
    override protected void Awake() 
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // HandleAllMovement();
    }

    public void HandleAllMovement() 
    {
        HandleMovement(); 
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput; 
        horizontalMovement = PlayerInputManager.instance.horizontalInput; 
        movementCombined = PlayerInputManager.instance.movementCombined;
    }

    private void HandleMovement() 
    {

        GetVerticalAndHorizontalInputs();
        if(!player.canMove)
            return;
        movementDirection = PlayerCamera.instance.transform.forward * verticalMovement; 
        movementDirection = movementDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        movementDirection.Normalize();
        movementDirection.y = 0;
        if(player.isSprinting) 
        {
            player.characterController.Move(movementDirection * sprintSpeed * Time.deltaTime);
        }
        else 
        {
            if(PlayerInputManager.instance.movementCombined > 0.5f)
            {
                player.characterController.Move(movementDirection * runningSpeed  * Time.deltaTime);
            }
            else if(PlayerInputManager.instance.movementCombined <= 0.5f)
            {
                player.characterController.Move(movementDirection * walkingSpeed * Time.deltaTime);
            }
        }
        
    }

    private void HandleRotation() 
    {
        if(!player.canRotate)
            return;
        targetDirection = Vector3.zero; 
        targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetDirection = targetDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetDirection.Normalize();
        targetDirection.y = 0; 

        if(targetDirection == Vector3.zero) 
        {
            targetDirection = transform.forward;
        }
        Quaternion newRotation = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void HandleSprint() 
    {
        if(player.isPerformingAction) 
        {
            player.isSprinting = false;
        }

        if(movementCombined > 0.5) 
        {
            player.isSprinting = true; 
        }
        else 
        {
            player.isSprinting = false;
        }
    }

    public void AttemptToPerformDodge() 
    {
        if(player.isPerformingAction) 
        {
            return;
        }
        if(PlayerInputManager.instance.movementCombined > 0) 
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

            rollDirection.y = 0;
            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward", true); 
        }
        else 
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("StepBack", true); 
        }
       

    }
    
}
