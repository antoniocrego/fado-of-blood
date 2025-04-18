using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterLocomotionManager : MonoBehaviour
{

    CharacterManager character;

    [SerializeField] LayerMask groundLayer; 

    [SerializeField] protected float gravityForce = -5.55f;

    [SerializeField] float groundCheckSphereRadius = 0.3f;
    [SerializeField] protected Vector3 yVelocity; 
    [SerializeField] protected float groundedYVelocity = -20;

    [SerializeField] protected float fallStartYVelocity = -5; 

    protected bool fallingVelocityHasBeenSet = false; 

    protected float inAirTime = 0;

    public void Update() 
    {
        HandleGroundCheck(); 
        if(character.isGrounded) 
        {
            if(yVelocity.y < 0) 
            {
                inAirTime = 0; 
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity; 
            }
        }
        else 
        {
            if(!character.isJumping && !fallingVelocityHasBeenSet) 
            {
                fallingVelocityHasBeenSet = true; 
                yVelocity.y = fallStartYVelocity;
            }

            inAirTime += Time.deltaTime;

            yVelocity.y += gravityForce * Time.deltaTime;

            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        character.characterController.Move(yVelocity * Time.deltaTime);
    }
    protected virtual void Awake() 
    {
        if(character == null) 
        {
            character = GetComponent<CharacterManager>();
        }
    }

    protected void HandleGroundCheck() 
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        if(character) 
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}
