using UnityEngine;
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
        movementDirection = PlayerCamera.instance.transform.forward * verticalMovement; 
        movementDirection = movementDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        movementDirection.Normalize();
        movementDirection.y = 0;

        if(PlayerInputManager.instance.movementCombined > 0.5f)
        {
            player.characterController.Move(movementDirection * runningSpeed  * Time.deltaTime);
        }
        else if(PlayerInputManager.instance.movementCombined <= 0.5f)
        {
            player.characterController.Move(movementDirection * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation() 
    {
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
    
}
