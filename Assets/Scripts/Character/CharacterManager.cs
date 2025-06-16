using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [HideInInspector] public CharacterStatsManager characterStatsManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;


    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    public CharacterGroup characterGroup;

    private float previousHealth;
    public float health = 100;
    public float stamina = 100;
    public bool isDead = false;

    public bool isGrounded = true;
    public bool isInvincible = false;

    public bool isPerformingAction = false;

    public bool isSprinting = false;

    public int maxStamina = 100;

    public int maxHealth = 100;

    public bool isLockedOn = false;

    public bool isMoving = false;

    private bool isChargingAttack = false;
    public bool isActive = true;

    public bool isInvulnerable = false;
    public bool isJumping = false;
    public bool isBlocking = false;
    public bool isAttacking = false;
    protected virtual void Start()
    {
        IgnoreMyOwnColliders();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inventoryManager = GetComponent<CharacterInventoryManager>();
        equipmentManager = GetComponent<CharacterEquipmentManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        playerUIHudManager = FindFirstObjectByType<PlayerUIHudManager>();
        previousHealth = health;
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

        // health <= 0 checks if was spawned in dead (no save scumming)
        if (health != previousHealth || health <= 0)
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

    public virtual void SetIsChargingAttack(bool isCharging)
    {
        isChargingAttack = isCharging;
        animator.SetBool("isChargingAttack", isCharging);
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
        characterSoundFXManager.PlayDeathSFX();
        yield return null;
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

    public void DisableAllDamageHitboxes()
    {
        DamageCollider[] colliders = GetComponentsInChildren<DamageCollider>();
        foreach (DamageCollider collider in colliders)
        {
            collider.DisableDamageCollider();
        }
    }
}