using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Attack")]
public class AIStateAttack : AIState
{
    [Header("Current Attack")]
    [HideInInspector] public AICharacterAttackAction currentAttack;
    [HideInInspector] public bool willPerformCombo = false;

    [Header("State Flags")]
    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool pivotAfterAttack = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null) return SwitchState(aiCharacter, aiCharacter.idleState);

        if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead) return SwitchState(aiCharacter, aiCharacter.idleState);

        aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhileAttacking(aiCharacter);
        
        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0,0,false,false);

        // perform combo if applicable
        if (willPerformCombo && !hasPerformedCombo)
        {
            if (currentAttack.comboAction != null)
            {
                //hasPerformedCombo = true;
                //currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
            }
        }

        if (aiCharacter.isPerformingAction) return this;

        if (!hasPerformedAttack)
        {
            if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0) return this;

            PerformAttack(aiCharacter);

            return this;
        }

        if (pivotAfterAttack)
            aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        
        return SwitchState(aiCharacter, aiCharacter.combatState);
    }

    protected void PerformAttack(AICharacterManager aiCharacter)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aiCharacter);
        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacterManager)
    {
        base.ResetStateFlags(aiCharacterManager);

        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }
}
