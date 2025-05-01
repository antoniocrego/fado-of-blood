using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Pursue")]
public class AIStatePursue : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) return this;
        
        if (aiCharacter.characterCombatManager.currentTarget == null) return SwitchState(aiCharacter, aiCharacter.idleState);

        if (!aiCharacter.navMeshAgent.enabled) aiCharacter.navMeshAgent.enabled = true;

        aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

        // TRY THIS OUT LATER ON
        aiCharacter.navMeshAgent.SetDestination(aiCharacter.characterCombatManager.currentTarget.transform.position);

        // NavMeshPath path = new NavMeshPath();
        // aiCharacter.navMeshAgent.CalculatePath(aiCharacter.characterCombatManager.currentTarget.transform.position, path);
        // aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }
}
