using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    protected CharacterManager character;

    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Attack Type")]
    public AttackType currentAttackType;

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
}
