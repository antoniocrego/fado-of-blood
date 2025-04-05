using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState 
    {
        WalkingLeft,

        WalkingRight,

        WalkingUp, 

        WalkingDown, 
        Idle, 
        Running,
        Attacking
    }

    public Animator movementAnimation;

    public PlayerState currentState; 
    public PlayerState newState;

    private Quaternion targetRotation;
    private float rotationSpeed = 500f;

    private bool isPLayerAttacking = false;

    private float attackTimer = 0f; 

    private float attackDuration = 2f; 
    

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
        if(!isPLayerAttacking) 
        {
            if(Input.anyKey) 
            {
                if(Input.GetKey(KeyCode.W)) 
                {
                    newState = PlayerState.WalkingUp; 
                } 
                if (Input.GetKey(KeyCode.D)) 
                {
                    transform.rotation *= Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime);
                    newState = PlayerState.WalkingRight; 
                } 
                if (Input.GetKey(KeyCode.A)) 
                {
                    transform.rotation *= Quaternion.Euler(Vector3.down * rotationSpeed * Time.deltaTime);
                    newState = PlayerState.WalkingLeft; 
                } 
                if(Input.GetKey(KeyCode.S)) 
                {
                    newState = PlayerState.WalkingDown; 
                }
                if(Input.GetKey(KeyCode.Space)) 
                {
                    newState = PlayerState.Attacking;
                }
            }
            
            else if(!isPLayerAttacking)
            {
                newState = PlayerState.Idle;    
            }
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
            currentState = newState; 
            switch(currentState) 
            {
                case PlayerState.WalkingLeft:
                    if(!inBoss) 
                    {
                        movementAnimation.SetInteger("state" ,2);
                    }
                    else 
                    {
                        movementAnimation.SetInteger("state" , 0);
                    } 
                    break;
                case PlayerState.WalkingRight:
                    if(!inBoss) 
                    {
                        movementAnimation.SetInteger("state" ,2);
                    }
                    else 
                    {
                        movementAnimation.SetInteger("state" , 1);
                    } 
                    break;
                case PlayerState.WalkingUp:
                    if(!inBoss) 
                    {
                        movementAnimation.SetInteger("state" ,2);
                    }
                    else 
                    {
                        movementAnimation.SetInteger("state" , 2);
                    }
                    break;
                case PlayerState.WalkingDown:
                    if(!inBoss) 
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        movementAnimation.SetInteger("state" ,2);
                    }
                    else 
                    {
                        movementAnimation.SetInteger("state" , 3);
                    }
                    break;

                case PlayerState.Idle
:
                    if(!inBoss) 
                    {
                        movementAnimation.SetInteger("state" ,4);
                    }
                    else 
                    {
                        movementAnimation.SetInteger("state" , 4);
                    } 
                    break;

                case PlayerState.Attacking:
                    if(!inBoss) 
                    {
                        isPLayerAttacking = true;
                        movementAnimation.SetInteger("state" ,5);
                    }
                    else 
                    {
                        isPLayerAttacking = true;
                        movementAnimation.SetInteger("state" , 5);
                    } 
                    break;
            }
        }

    }

    public void rotatePLayerToTarget() 
    {
        if(currentState != PlayerState.Idle) 
        {
            Quaternion target = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = target; 
        }
    }
    void Update()
    {
        handleKeyInput(); 
        handleStateChange();
        // rotatePLayerToTarget();
        
    }
}
