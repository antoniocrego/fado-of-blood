using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Combat")]
public class AIStateCombat : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks;
    protected List<AICharacterAttackAction> potentialAttacks;
    private AICharacterAttackAction chosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    protected bool hasRolledForComboChance = false;

    [Header("Engagement Distance")]
    [SerializeField] public float maximumEngagementDistance = 5f;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) return this;

        if (!aiCharacter.navMeshAgent.enabled) aiCharacter.navMeshAgent.enabled = true;

        if (!aiCharacter.isMoving){
            if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idleState);
            
        if (!hasAttack){
            GetNewAttack(aiCharacter);
        }
        else{
            aiCharacter.attackState.currentAttack = chosenAttack;

            return SwitchState(aiCharacter, aiCharacter.attackState);
        }
        
        if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance){
            return SwitchState(aiCharacter, aiCharacter.pursueState);
        }

        aiCharacter.navMeshAgent.SetDestination(aiCharacter.characterCombatManager.currentTarget.transform.position);

        return this;
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

        foreach (AICharacterAttackAction attack in aiCharacterAttacks)
        {
            // we're too far or too close to the target
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > attack.maximumAttackDistance || aiCharacter.aiCharacterCombatManager.distanceFromTarget < attack.minimumAttackDistance)
                continue;
            
            // we're not in the right angle to attack
            if (aiCharacter.aiCharacterCombatManager.viewableAngle < attack.minimumAttackAngle || aiCharacter.aiCharacterCombatManager.viewableAngle > attack.maximumAttackAngle)
                continue;
            
            potentialAttacks.Add(attack);
        }   

        if (potentialAttacks.Count <= 0) return;

        var totalWeight = 0;

        foreach (AICharacterAttackAction attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight+1);
        var processedWeight = 0;

        foreach (AICharacterAttackAction attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if (randomWeightValue <= processedWeight)
            {
                // found and chose valid attack
                chosenAttack = attack;
                previousAttack = chosenAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int outcomeChance){
        bool outcomeWillBePerformed = false;
        
        if (Random.Range(0, 100) < outcomeChance)
        {
            outcomeWillBePerformed = true;
        }
        return outcomeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);

        hasRolledForComboChance = false;
        hasAttack = false;
    }
}