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
    private float rotationSpeed = 200f;
    

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
            if(Input.GetKey(KeyCode.W)) 
            {
                newState = PlayerState.WalkingUp; 
            } 
            else if (Input.GetKey(KeyCode.D)) 
            {
                newState = PlayerState.WalkingRight; 
            } 
            else if (Input.GetKey(KeyCode.A)) 
            {
                newState = PlayerState.WalkingLeft; 
            } 
            else if(Input.GetKey(KeyCode.S)) 
            {
                newState = PlayerState.WalkingDown; 
            }
            else 
            {
                newState = PlayerState.Idle;    
            }
        
    } 

    void handleStateChange() 
    {
        if(currentState != newState) 
        {
            currentState = newState; 
            switch(currentState) 
            {
                case PlayerState.WalkingLeft:
                    if(!inBoss) 
                    {
                        targetRotation = Quaternion.Euler(0, -90, 0);
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
                        targetRotation = Quaternion.Euler(0, 90, 0);
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
                        targetRotation = Quaternion.Euler(0, 0, 0);
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
                        targetRotation = Quaternion.Euler(0, 180, 0);
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
        rotatePLayerToTarget();
        
    }
}
