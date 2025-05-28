using System.Collections;
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

    private float previousHealth;
    public float health;
    public float stamina;
    public bool isDead = false;
    public bool isInvincible = false;

    public bool isPerformingAction = false;

    public bool isSprinting = false;

    public int endurance = 1;

    public int vitality = 1;
    public int maxStamina = 0;

    public int maxHealth = 0;

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
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        health = 100;
        previousHealth = health;
        maxHealth = 100;
        stamina = 10;
        characterCombatManager = GetComponent<CharacterCombatManager>();
        playerUIHudManager = FindObjectOfType<PlayerUIHudManager>();
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            return;
        }
        
        if (health != previousHealth)
        {
            CheckHealth();
        }

        previousHealth = health;
    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    public virtual IEnumerator ProcessDeath()
    {
        this.health = 0;
        this.isDead = true;

        // Disable character controller
        this.characterLocomotionManager.canMove = false;
        this.characterLocomotionManager.canRotate = false;
        this.isPerformingAction = false;
        this.isSprinting = false;
        this.isMoving = false;

        characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);

        // Play death SFX
        yield return new WaitForSeconds(5);
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            if (!isDead)
            {
                StartCoroutine(ProcessDeath());
            }
        }
        else if (health > maxHealth)
        {
            // health = maxHealth; // Ensure health does not exceed maxHealth
        }
    }
}