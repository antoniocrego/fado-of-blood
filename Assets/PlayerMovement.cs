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
        Attacking,

        Turning
    }

    public Animator movementAnimation;

    public PlayerState currentState; 
    public PlayerState newState;

    public PlayerState nextState; 

    public PlayerState previousStateBeforeIdle; 

    private Quaternion targetRotation;
    private float rotationSpeed = 1000f;

    public float acceleration = 10.0f;

    private bool isPLayerAttacking = false;

    public bool isTurning = false;

    private float turningTimer = 0f; 

    public float turningDuration = 0.6f;

    private float attackTimer = 0f; 

    private float attackDuration = 2f; 

    public Transform mainCamera; 

    public Vector3 playerDirection;

    private Vector3 movementDirection;

    public bool intermediateAnimation = false;
    

    public bool inBoss = false; 

    public float cameraCooldownTimer = 0f;

    public float cameraCooldownDuration = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = PlayerState.Idle;
        newState = PlayerState.Idle;
        nextState = PlayerState.Idle;
    }

    // Update is called once per frame
    void handleKeyInput() 
    {

        if(isTurning || isPLayerAttacking)
        {
            return;
        }

        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        playerDirection = new Vector3(horizontal, 0, vertical).normalized;

        Vector3 cameraDirection = mainCamera.forward;
        cameraDirection.y = 0;
        cameraDirection.Normalize();

        Vector3 cameraRight = mainCamera.right;
        cameraRight.y = 0;
        cameraRight.Normalize();


        movementDirection = cameraDirection * vertical + cameraRight * horizontal; 

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Space))
        {
            if(isTurning) 
            {
                return;
            }
            if(Input.GetKey(KeyCode.A)) 
            {
                if(currentState == PlayerState.WalkingRight || (currentState == PlayerState.Idle && previousStateBeforeIdle == PlayerState.WalkingRight))
                {
                    nextState = PlayerState.WalkingLeft;
                    newState = PlayerState.Turning;
                }
                else 
                {
                    newState = PlayerState.WalkingLeft; 
                }
                previousStateBeforeIdle = PlayerState.Idle;
            }
            else if(Input.GetKey(KeyCode.D)) 
            {
                if(currentState == PlayerState.WalkingLeft || (currentState == PlayerState.Idle && previousStateBeforeIdle == PlayerState.WalkingLeft)) 
                {
                    nextState = PlayerState.WalkingRight;
                    newState = PlayerState.Turning;
                }
                else 
                {
                    newState = PlayerState.WalkingRight; 
                }
                previousStateBeforeIdle = PlayerState.Idle;
            }
            if(Input.GetKey(KeyCode.W)) 
            {
                if(currentState == PlayerState.WalkingBackward || (currentState == PlayerState.Idle && previousStateBeforeIdle == PlayerState.WalkingBackward))
                {
                    nextState = PlayerState.WalkingForward;
                    newState = PlayerState.Turning;
                }
                else if(!isTurning)
                {
                    newState = PlayerState.WalkingForward; 
                }
                previousStateBeforeIdle = PlayerState.Idle;
            }
            else if(Input.GetKey(KeyCode.S)) 
            {
                if(currentState == PlayerState.WalkingForward || (currentState == PlayerState.Idle && previousStateBeforeIdle == PlayerState.WalkingForward))
                {
                    nextState = PlayerState.WalkingBackward;
                    newState = PlayerState.Turning;
                }
                else if(!isTurning)
                {
                    newState = PlayerState.WalkingBackward; 
                }
                previousStateBeforeIdle = PlayerState.Idle;
            }
            if(Input.GetKey(KeyCode.Space)) 
            {
                newState = PlayerState.Attacking; 
            }
        }
        else if(!isPLayerAttacking && !isTurning) 
        {
            if(newState != PlayerState.Idle) 
            {
                previousStateBeforeIdle = newState;
            }
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
        else if(isTurning)
        {
            turningTimer += Time.deltaTime; 
            if(turningTimer >= turningDuration) 
            {
                Debug.Log("the new state is " + newState);
                newState = nextState;
                nextState = PlayerState.Idle;
                turningTimer = 0f; 
                previousStateBeforeIdle = PlayerState.Idle;
                checkCurrentState();
                isTurning = false;
                cameraCooldownTimer = 0f;
            }
        }
        else if(currentState != newState) 
        {
            checkCurrentState();
        }

    }

    void checkCurrentState() 
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
                case PlayerState.Turning: 
                    isTurning = true;
                    movementAnimation.SetInteger("state", 6); 
                    break;
            }
            currentState = newState; 
    }

    public void handleCameraRotation() 
    {
        if(cameraCooldownTimer < cameraCooldownDuration) 
        {
            cameraCooldownTimer += Time.deltaTime; 
            return; 
        }
        if(movementDirection.magnitude >= 0.1f && !isTurning)
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
