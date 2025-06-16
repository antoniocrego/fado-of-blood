using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;

    [Header("Weapon Attack Modifiers")]
    public float light_Attack_01_Modifier;
    public float light_Attack_02_Modifier;
    public float heavy_Attack_01_Modifier;
    public float heavy_Attack_02_Modifier;
    public float charge_Attack_01_Modifier;
    public float charge_Attack_02_Modifier;
    public float running_Attack_01_Modifier;
    public float rolling_Attack_01_Modifier;
    public float backstep_Attack_01_Modifier;

    protected override void Awake()
    {
        base.Awake();

        if (damageCollider == null)
        {
            damageCollider = GetComponent<Collider>();
        }
        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered: " + other.gameObject.name);
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();
        Debug.Log("Damage target: " + damageTarget);
        if (damageTarget != null)
        {
            if (damageTarget == characterCausingDamage)
            {
                return; // Ignore self-damage
            }
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
    }
    
    protected override void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = characterCausingDamage.transform.position - damageTarget.transform.position;
        dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if (charactersDamaged.Contains(damageTarget))
        {
            return;
        }

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.damage = damage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        switch (characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.LightAttack02:
                ApplyAttackDamageModifiers(light_Attack_02_Modifier, damageEffect);
                break;
            case AttackType.HeavyAttack01:
                ApplyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.HeavyAttack02:
                ApplyAttackDamageModifiers(heavy_Attack_02_Modifier, damageEffect);
                break;
            case AttackType.ChargedAttack01:
                ApplyAttackDamageModifiers(charge_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.ChargedAttack02:
                ApplyAttackDamageModifiers(charge_Attack_02_Modifier, damageEffect);
                break;
            case AttackType.RunningAttack01:
                ApplyAttackDamageModifiers(running_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.RollingAttack01:
                ApplyAttackDamageModifiers(rolling_Attack_01_Modifier, damageEffect);
                break;
            case AttackType.BackstepAttack01:
                ApplyAttackDamageModifiers(backstep_Attack_01_Modifier, damageEffect);
                break;
            default:
                break;
        }

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
    {
        damage.damage *= modifier;
    }
}
