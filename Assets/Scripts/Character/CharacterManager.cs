using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public CharacterController characterController;

    [HideInInspector] public Animator animator;
    public CharacterInventoryManager inventoryManager;
    public CharacterEquipmentManager equipmentManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    public float health;
    public float stamina;
    public bool isDead = false;

    public bool isPerformingAction = false;

    public bool canRotate = true; 

    public bool canMove = true;

    public bool applyRootMotion = false;

    public bool isSprinting = false;

    public int endurance = 1; 
    public int maxStamina = 0;

    public bool isJumping = false;

    public bool isGrounded = true;

    public bool isLockedOn = false; 

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryManager = GetComponent<CharacterInventoryManager>();
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        health = 100;
        stamina = 10;
    }

    protected virtual void Update(){
        
    }

    protected virtual void LateUpdate(){
           
    }
}