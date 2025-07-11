using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public PlayerManager player;

    public Vector3 targetPosition; 
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

    [SerializeField] private float spritingStaminaCost = 10f;

    [SerializeField] private float sprintingRecovery = 5f;

    [SerializeField] float minimumStaminaToStartSprinting = 10f;

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

    public void HandleAllMovement() 
    {
        HandleMovement(); 
        HandleRotation();
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
        if(!player.characterLocomotionManager.canMove)
            return;

         if (player.isLockedOn && player.playerCombatManager.currentTarget != null)
        {
            Vector3 playerForward = player.transform.forward;
            Vector3 playerRight = player.transform.right;
            movementDirection = playerForward * verticalMovement + playerRight * horizontalMovement;
        }
        else
        {
            Vector3 camForward = PlayerCamera.instance.transform.forward;
            Vector3 camRight = PlayerCamera.instance.transform.right;
            movementDirection = camForward * verticalMovement + camRight * horizontalMovement;
        }

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
            else if(PlayerInputManager.instance.movementCombined > 0f && PlayerInputManager.instance.movementCombined <= 0.5f)
            {
                player.characterController.Move(movementDirection * walkingSpeed * Time.deltaTime);
            }
        }
        
    }

    private void HandleRotation() 
    {
        if (player.isDead)
            return;
        if(!player.characterLocomotionManager.canRotate)
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
        if(!player.isLockedOn || player.isSprinting)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
        else if(player.isLockedOn)
        {
            if(player.playerCombatManager.currentTarget != null) 
            {
                Vector3 directionToTarget = player.playerCombatManager.currentTarget.transform.position - transform.position;
                directionToTarget.y = 0;
                directionToTarget.Normalize();
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else 
            {
                player.isLockedOn = false; 
                PlayerCamera.instance.isCameraLocked = false;
                player.playerCombatManager.currentTarget = null;
            }
        }
    }

    public void HandleSprint() 
    {
        bool canSprintConditionsMet = PlayerInputManager.instance.sprintInput &&
                                      !player.isPerformingAction &&
                                      PlayerInputManager.instance.movementCombined > 0.5f;

        if (player.isSprinting)
        {
            if (!canSprintConditionsMet || player.stamina <= 0)
            {
                player.isSprinting = false;
            }
        }
        else
        {
            if (canSprintConditionsMet && player.stamina > minimumStaminaToStartSprinting)
            {
                player.isSprinting = true;
            }
        }

        if (player.isSprinting)
        {
            player.stamina -= spritingStaminaCost * Time.deltaTime;
            if (player.stamina < 0) 
            {
                player.stamina = 0;
                player.isSprinting = false; 
            }
        }
    }

    private void HandleFreeFallMovement() 
    {
        if(!player.characterLocomotionManager.isGrounded) 
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
        if(!player.characterLocomotionManager.isGrounded) 
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
