using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DamageCollider : MonoBehaviour
{

    [Header("Collider Owner")]
    [SerializeField] protected CharacterManager colliderOwner;

    [Header("Damage Collider")]
    [SerializeField] protected Collider damageCollider;

    [Header("Damage")]
    public float damage = 0;

    [Header("Poise")]
    public float poiseDamage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Characters Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    [Header("Block")]
    protected Vector3 directionFromAttackToDamageTarget;
    protected float dotValueFromAttackToDamageTarget;

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Trigger entered: " + other.gameObject.name);
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
        // Debug.Log("Damage target: " + damageTarget);
        if (damageTarget != null && damageTarget != colliderOwner)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            CheckForBlock(damageTarget);
            DamageTarget(damageTarget);
        }
    }
    
    protected virtual void CheckForBlock(CharacterManager damageTarget)
    {
        //  IF THIS CHARACTER HAS ALREADY BEEN DAMAGED, DO NOT PROCEED
        if (charactersDamaged.Contains(damageTarget))
            return;

        GetBlockingDotValues(damageTarget);

        // 1. CHECK IF THE CHARACTER BEING DAMAGED IS BLOCKING
        if (damageTarget.isBlocking && dotValueFromAttackToDamageTarget > 0.3f)
        {
            // 2. IF THE CHARACTER IS BLOCKING, CHECK IF THEY ARE FACING IN THE CORRECT DIRECTION TO BLOCK SUCCESSFULLY

            charactersDamaged.Add(damageTarget);

            TakeBlockedDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);

            damageEffect.damage = damage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.staminaDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;

            // 3. APPLY BLOCKED CHARACTER DAMAGE TO TARGET
            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }

    protected virtual void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        if (charactersDamaged.Contains(damageTarget))
        {
            return;
        }

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.damage = damage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(colliderOwner.transform.forward, damageTarget.transform.forward, Vector3.up);

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear(); // Reset the characters that have been hit, so they can be hit again in the next attack
    }

    protected virtual void Awake()
    {
        colliderOwner = GetComponentInParent<CharacterManager>();
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = damageCollider.enabled ? Color.red : Color.green;

    //     Gizmos.DrawWireCube(damageCollider.bounds.center, damageCollider.bounds.size);
    // }
}