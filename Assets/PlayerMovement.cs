using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState 
    {
        WalkingLeft,

        WalkingRight,

        WalkingForward,

        WalkingBackward, 
        Idle, 
        Running,
        Attacking
    }

    public Animator movementAnimation;

    public PlayerState currentState; 
    public PlayerState newState;

    public PlayerState previousState; 

    private Quaternion targetRotation;
    private float rotationSpeed = 1000f;

    public float acceleration = 10.0f;

    private bool isPLayerAttacking = false;

    private float attackTimer = 0f; 

    private float attackDuration = 2f; 

    public Transform mainCamera; 

    public Vector3 playerDirection;

    private Vector3 movementDirection;
    

    public bool inBoss = false; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = PlayerState.Idle;
        newState = PlayerState.Idle;
        previousState = PlayerState.Idle;
    }

    // Update is called once per frame
    void handleKeyInput() 
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementAnimation.SetFloat("X", horizontal);
        movementAnimation.SetFloat("Y", vertical);

        Debug.Log("Horizontal: " + horizontal + ", Vertical: " + vertical);

        playerDirection = new Vector3(horizontal, 0, vertical).normalized;

        Vector3 cameraDirection = mainCamera.forward;
        cameraDirection.y = 0;
        cameraDirection.Normalize();

        Vector3 cameraRight = mainCamera.right;
        cameraRight.y = 0;
        cameraRight.Normalize();


        movementDirection = cameraDirection * vertical + cameraRight * horizontal; 

        if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Space)) && !isPLayerAttacking) 
        {
            if(Input.GetKey(KeyCode.A)) 
            {
                newState = PlayerState.WalkingLeft;
            }
            else if(Input.GetKey(KeyCode.D)) 
            {
                newState = PlayerState.WalkingRight; 
            }
            if(Input.GetKey(KeyCode.W)) 
            {
                newState = PlayerState.WalkingForward; 
            }
            else if(Input.GetKey(KeyCode.S)) 
            {
                newState = PlayerState.WalkingBackward; 
            }
            if(Input.GetKey(KeyCode.Space)) 
            {
                newState = PlayerState.Attacking; 
            }
        }
        else if(!isPLayerAttacking) 
        {
            previousState = newState;
            newState = PlayerState.Idle; 
        }
    } 

    void handleStateChange() 
    {
        if(isPLayerAttacking) 
        {
            attackTimer += Time.deltaTime; 
            if(attackTimer >= attackDuration) 
            {
                isPLayerAttacking = false; 
                attackTimer = 0f; 
            }
        }
        else if(currentState != newState) 
        {
            switch(newState) 
            {
                case PlayerState.WalkingLeft: 
                    movementAnimation.SetInteger("state", 0); 
                    break;
                case PlayerState.WalkingRight:
                    movementAnimation.SetInteger("state", 1); 
                    break;
                case PlayerState.WalkingForward: 
                    movementAnimation.SetInteger("state", 2);
                    break;
                case PlayerState.WalkingBackward:
                    movementAnimation.SetInteger("state", 3); 
                    break;  
                case PlayerState.Idle: 
                    movementAnimation.SetInteger("state", 4);
                    break;
                case PlayerState.Attacking: 
                    isPLayerAttacking = true;
                    movementAnimation.SetInteger("state", 5); 
                    break;
            }
            currentState = newState; 
        }

    }

    public void handleCameraRotation() 
    {
        if(movementDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
         if(Input.GetKey(KeyCode.LeftShift)) 
            {
                movementAnimation.SetBool("sprint", true); 
            }
        if(Input.GetKeyUp(KeyCode.LeftShift)) 
            {
                movementAnimation.SetBool("sprint", false); 
            }
        handleKeyInput(); 
        handleStateChange();
        handleCameraRotation();
        
    }
}
