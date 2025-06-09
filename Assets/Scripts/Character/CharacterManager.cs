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
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    public CharacterGroup characterGroup;
    public float health;
    public float stamina;
    public bool isDead = false;
    public bool isGrounded = true;
    public bool isPerformingAction = false;

    public bool isSprinting = false;

    public int endurance = 1;

    public int vitality = 1;
    public int maxStamina = 0;

    public int maxHealth = 0;

    public bool isLockedOn = false;

    public bool isMoving = false;

    private bool isChargingAttack = false;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryManager = GetComponent<CharacterInventoryManager>();
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        health = 100;
        stamina = 10;
        characterCombatManager = GetComponent<CharacterCombatManager>();
        playerUIHudManager = FindObjectOfType<PlayerUIHudManager>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }
    
    public virtual void SetIsChargingAttack(bool isCharging)
    {
        isChargingAttack = isCharging;
        animator.SetBool("isChargingAttack", isCharging);
    }
}