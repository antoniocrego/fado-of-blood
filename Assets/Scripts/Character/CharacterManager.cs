using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public CharacterController characterController;

    [HideInInspector] public Animator animator;
    public CharacterInventoryManager inventoryManager;
    public CharacterEquipmentManager equipmentManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    public CharacterGroup characterGroup;
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

    public bool isMoving = false;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryManager = GetComponent<CharacterInventoryManager>();
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        health = 100;
        stamina = 10;
        characterCombatManager = GetComponent<CharacterCombatManager>();
    }

    protected virtual void Update(){
        
    }

    protected virtual void FixedUpdate(){
    
    }

    protected virtual void LateUpdate(){
           
    }
}