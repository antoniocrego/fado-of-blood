using Unity.VisualScripting;
using UnityEngine;

public class SoldierCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] SoldierSwordDamageCollider swordDamageCollider;
    [SerializeField] SoldierFootDamageCollider footDamageCollider;

    [Header("Damage")]
    [SerializeField] float baseDamage = 10f;
    [SerializeField] float basePoiseDamage = 10f;
    [SerializeField] float slash01DamageMultiplier = 1f;
    [SerializeField] float slash02Part1DamageMultiplier = 1.5f;
    [SerializeField] float slash02Part2DamageMultiplier = 0.8f;
    [SerializeField] float slash02Part3DamageMultiplier = 2.5f;
    [SerializeField] float spin01DamageMultiplier = 2f;
    [SerializeField] float spin02DamageMultiplier = 1.5f;
    [SerializeField] float kickDamageMultiplier = 0.5f;

    public void SetSlash01Damage()
    {
        // we could have both calculations here, but since each attack only uses 1 hand, its not worth it
        swordDamageCollider.damage = baseDamage * slash01DamageMultiplier;
        swordDamageCollider.poiseDamage = basePoiseDamage * slash01DamageMultiplier;
    }

    public void SetSlash02Part1Damage()
    {
        swordDamageCollider.damage = baseDamage * slash02Part1DamageMultiplier;
        swordDamageCollider.poiseDamage = basePoiseDamage * slash02Part1DamageMultiplier;
    }

    public void SetSlash02Part2Damage()
    {
        swordDamageCollider.damage = baseDamage * slash02Part2DamageMultiplier;
        swordDamageCollider.poiseDamage = basePoiseDamage * slash02Part2DamageMultiplier;
    }

    public void SetSlash02Part3Damage()
    {
        swordDamageCollider.damage = baseDamage * slash02Part3DamageMultiplier;
        swordDamageCollider.poiseDamage = basePoiseDamage * slash02Part3DamageMultiplier;
    }

    public void SetSpin01Damage()
    {
        swordDamageCollider.damage = baseDamage * spin01DamageMultiplier;
        swordDamageCollider.poiseDamage = basePoiseDamage * spin01DamageMultiplier;
    }

    public void SetSpin02Damage()
    {
        swordDamageCollider.damage = baseDamage * spin02DamageMultiplier;
        swordDamageCollider.poiseDamage = basePoiseDamage * spin02DamageMultiplier;
    }

    public void SetKickDamage()
    {
        footDamageCollider.damage = baseDamage * kickDamageMultiplier;
        footDamageCollider.poiseDamage = basePoiseDamage * kickDamageMultiplier;
    }

    public void OpenSwordDamageCollider()
    {
        swordDamageCollider.EnableDamageCollider();
    }

    public void CloseSwordDamageCollider()
    {
        swordDamageCollider.DisableDamageCollider();
    }

    public void OpenFootDamageCollider()
    {
        footDamageCollider.EnableDamageCollider();
    }

    public void CloseFootDamageCollider()
    {
        footDamageCollider.DisableDamageCollider();
    }

    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) return;

        if (viewableAngle >= 60 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 R", true);
        }
        else if (viewableAngle <= -60 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 L", true);
        }
        else if (viewableAngle > 110 || viewableAngle < -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 180", true);
        }
    }
}
