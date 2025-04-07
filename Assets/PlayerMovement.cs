using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState 
    {
        Walking, 
        Idle, 
        Running,
        Attacking
    }

    public Animator movementAnimation;

    public PlayerState currentState; 
    public PlayerState newState;

    private Quaternion targetRotation;
    private float rotationSpeed = 1000f;

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
    }

    // Update is called once per frame
    void handleKeyInput() 
    {
        
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Debug.Log("Horizontal: " + horizontal + ", Vertical: " + vertical);

        playerDirection = new Vector3(horizontal, 0, vertical).normalized;

        Vector3 cameraDirection = mainCamera.forward;
        cameraDirection.y = 0;
        cameraDirection.Normalize();

        Vector3 cameraRight = mainCamera.right;
        cameraRight.y = 0;
        cameraRight.Normalize();


        movementDirection = cameraDirection * vertical + cameraRight * horizontal; 

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) 
        {
            newState = PlayerState.Walking;
        }
        else 
        {
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
                case PlayerState.Walking: 
                    movementAnimation.SetInteger("state", 2);
                    break; 
                case PlayerState.Idle: 
                    movementAnimation.SetInteger("state", 4);
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
        handleKeyInput(); 
        handleStateChange();
        handleCameraRotation();
        
    }
}
