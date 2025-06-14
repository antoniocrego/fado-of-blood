using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;


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

    public int vitality = 10;
    public int maxStamina = 0;

    public int maxHealth = 0;

    public bool isLockedOn = false;

    public bool isMoving = false;

    public bool isActive = true;

    public bool isInvulnerable = false;

    protected virtual void Start()
    {
        DontDestroyOnLoad(this);
        IgnoreMyOwnColliders();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryManager = GetComponent<CharacterInventoryManager>();
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        health = 100;
        previousHealth = health;
        maxHealth = 100;
        stamina = 10;
        characterCombatManager = GetComponent<CharacterCombatManager>();
        playerUIHudManager = FindObjectOfType<PlayerUIHudManager>();
    }

    protected virtual void Update()
    {
        if (!isActive)
        {
            gameObject.SetActive(false);
            return;
        }

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

    public virtual IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
    {
        health = 0;
        isDead = true;

        // Disable character controller
        characterLocomotionManager.canMove = false;
        characterLocomotionManager.canRotate = false;
        isPerformingAction = false;
        isSprinting = false;
        isMoving = false;

        if (!manuallySelectDeathAnimation)
        {
            // Play death animation
            characterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
        }


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

    protected virtual void IgnoreMyOwnColliders()
    {
        Collider characterCollider = GetComponent<Collider>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        List<Collider> ignoreColliders = new List<Collider>();

        foreach (Collider collider in colliders)
        {
            ignoreColliders.Add(collider);
        }

        ignoreColliders.Add(characterCollider);

        foreach (Collider collider in ignoreColliders)
        {
            foreach (Collider otherCollider in colliders)
            {
                if (otherCollider != collider)
                {
                    Physics.IgnoreCollision(collider, otherCollider);
                }
            }
        }
    }
}