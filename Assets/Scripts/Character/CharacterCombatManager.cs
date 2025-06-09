using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    protected CharacterManager character;

    [Header("Last Attack Animation Performed")]
    public string lastAttackAnimationPerformed;

    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Attack Flags")]
    public bool canPerformRollingAttack = false;
    public bool canPerformBackstepAttack = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }

    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (newTarget != null)
        {
            currentTarget = newTarget;
        }
        else
        {
            currentTarget = null;
        }
    }
    
    public void EnableIsInvulnerable()
    {
        character.isInvulnerable = true;
    }

    public void DisableIsInvulnerable()
    {
        character.isInvulnerable = false;
    }

    public void EnableCanDoRollingAttack()
    {
        canPerformRollingAttack = true;
    }

    public void DisableCanDoRollingAttack()
    {
        canPerformRollingAttack = false;
    }

    public void EnableCanDoBackstepAttack()
    {
        canPerformBackstepAttack = true;
    }

    public void DisableCanDoBackstepAttack()
    {
        canPerformBackstepAttack = false;
    }

    public virtual void EnableCanDoCombo()
    {

    }

    public virtual void DisableCanDoCombo()
    {

    }
}
