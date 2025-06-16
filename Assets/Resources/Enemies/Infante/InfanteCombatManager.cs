using UnityEngine;

public class InfanteCombatManager : AICharacterCombatManager
{
    AIBossCharacterManager aiBossCharacterManager;

    [Header("Damage Colliders")]
    [SerializeField] InfanteSwordDamageCollider swordDamageCollider;

    [Header("Damage")]
    [SerializeField] float baseDamage = 30f;
    protected override void Awake()
    {
        base.Awake();
        aiBossCharacterManager = GetComponent<AIBossCharacterManager>();
    }

    public void Set360_01_Damage()
    {
        swordDamageCollider.damage = baseDamage * 2f;
    }

    public void Set360_02_Damage()
    {
        swordDamageCollider.damage = baseDamage * 1.5f;
    }

    public void Set_Upwards_Slash_Damage()
    {
        swordDamageCollider.damage = baseDamage * 1.5f;
    }

    public void Set_Downwards_Slash_Damage()
    {
        swordDamageCollider.damage = baseDamage * 1.2f;
    }

    public void Set_Slash_01_Damage()
    {
        swordDamageCollider.damage = baseDamage * 1f;
    }

    public void Set_Combo_01_Damage()
    {
        swordDamageCollider.damage = baseDamage * 3f;
    }

    public void Set_Combo_02_Damage()
    {
        swordDamageCollider.damage = baseDamage * 2f;
    }

    public void Set_Combo_03_Damage()
    {
        swordDamageCollider.damage = baseDamage * 1.8f;
    }
    

    public void OpenSwordDamageCollider()
    {
        swordDamageCollider.EnableDamageCollider();
    }

    public void CloseSwordDamageCollider()
    {
        swordDamageCollider.DisableDamageCollider();
    }

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
