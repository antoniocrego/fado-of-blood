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

    private Vector3 freeFallDirection;


    private Vector3 targetDirection;

    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float runningSpeed = 5f; 

    [SerializeField] private float walkingSpeed = 2f;

    [SerializeField] private float sprintSpeed = 7f;

    [SerializeField] private float spritingStaminaCost = 1f;

    [SerializeField] private float sprintingRecovery = 1f;

    [SerializeField] private float jumpHeight = 1f;

    [SerializeField] float jumpForwardSpeed = 5; 

    [SerializeField] float freeFallSpeed = 2;

    private Vector3 jumpDirection; 



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
        base.Update();
        // HandleAllMovement();
    }

    public void HandleAllMovement() 
    {
        HandleMovement(); 
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();
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
            if(player.stamina < player.maxStamina) 
            {
                player.stamina += sprintingRecovery * Time.deltaTime;
            }
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
        if(!player.isLockedOn)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

    public void HandleSprint() 
    {
        if(player.isPerformingAction) 
        {
            player.isSprinting = false;
        }
        if(player.stamina <= 0) 
        {
            player.isSprinting = false;
            return;
        }

        if(movementCombined > 0.5) 
        {
            player.isSprinting = true; 
        }
        else 
        {
            player.isSprinting = false;
        }

        if(player.isSprinting) 
        {
            player.stamina -= spritingStaminaCost * Time.deltaTime;
        }
    }

    public void AttemptToPerformJump() 
    {
        if(player.isPerformingAction) 
        {
            return;
        }

        player.playerAnimatorManager.PlayTargetActionAnimation("Jumping", false);

        player.isJumping = true;

        jumpDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
        jumpDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
        jumpDirection.Normalize();
        jumpDirection.y = 0;

        if(jumpDirection != Vector3.zero) 
        {
            if(player.isSprinting) 
            {
                jumpDirection *= 1;
            }
            else if(PlayerInputManager.instance.movementCombined > 0.5f) 
            {
                jumpDirection *= 0.5f;
            }
            else if(PlayerInputManager.instance.movementCombined <= 0.5f) 
            {
                jumpDirection *= 0.25f;
            }
        }
    }

    public void HandleJumpingMovement() 
    {
        if(player.isJumping)
        {
            player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement() 
    {
        if(!player.isGrounded) 
        {
            freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
            freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
            freeFallDirection.Normalize();
            freeFallDirection.y = 0;

            player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);

            
        }
    }

    public void ApplyJumpVelocity() 
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce);

    }

    public void AttemptToPerformDodge() 
    {
        if(player.isPerformingAction) 
        {
            return;
        }
        if(!player.isGrounded) 
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
