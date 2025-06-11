using UnityEngine;

public class AIAdamastorCombatManager : AICharacterCombatManager
{
    AIBossCharacterManager aiBossCharacterManager;

    [Header("Damage Colliders")]
    [SerializeField] AdamastorFistDamageCollider rightHandDamageCollider;
    [SerializeField] AdamastorFistDamageCollider leftHandDamageCollider;
    [SerializeField] AdamastorLegDamageCollider rightLegDamageCollider;
    //[SerializeField] DamageCollider fullBodyDamageCollider;

    [Header("Damage")]
    [SerializeField] float baseDamage = 10f;
    [SerializeField] float attack01DamageMultiplier = 1.5f;
    [SerializeField] float attack02DamageMultiplier = 3f;

    protected override void Awake()
    {
        base.Awake();
        aiBossCharacterManager = GetComponent<AIBossCharacterManager>();
    }

    public void SetAttack01Damage()
    {
        leftHandDamageCollider.damage = baseDamage * attack01DamageMultiplier;
    }

    public void SetAttack02Damage()
    {
        rightLegDamageCollider.damage = baseDamage * attack02DamageMultiplier;
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

    public void OpenRightLegDamageCollider()
    {
        rightLegDamageCollider.EnableDamageCollider();
    }

    public void CloseRightLegDamageCollider()
    {
        rightLegDamageCollider.DisableDamageCollider();
    }

    // public void OpenFullBodyDamageCollider()
    // {
    //     fullBodyDamageCollider.EnableDamageCollider();
    // }

    // public void CloseFullBodyDamageCollider()
    // {
    //     fullBodyDamageCollider.DisableDamageCollider();
    // }

    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) return;

        else if (viewableAngle >= 60 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 R", true);
        }
        else if (viewableAngle >= 145 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 180 R", true);
        }
        else if (viewableAngle <= -60 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 90 L", true);
        }
        else if (viewableAngle <= -145 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn 180 L", true);
        }
    }
}
