using UnityEngine;

public class AIUndeadCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] UndeadHandDamageCollider rightHandDamageCollider;
    [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;

    [Header("Damage")]
    [SerializeField] float baseDamage = 10f;
    [SerializeField] float attack01DamageMultiplier = 1f;
    [SerializeField] float attack02DamageMultiplier = 1.5f;

    public void SetAttack01Damage()
    {
        // we could have both calculations here, but since each attack only uses 1 hand, its not worth it
        rightHandDamageCollider.damage = baseDamage * attack01DamageMultiplier;
    }

    public void SetAttack02Damage()
    {
        rightHandDamageCollider.damage = baseDamage * attack02DamageMultiplier;
    }

    public void OpenRightHandDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }

    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) return;

        if (viewableAngle >= 40 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 R", true);
        }
        else if (viewableAngle <= -40 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 L", true);
        }

    }
}
