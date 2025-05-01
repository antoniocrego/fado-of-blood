using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class AIStateIdle : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.characterCombatManager.currentTarget != null)
        {
            return this;
        }
        else{
            aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
            return this;
        }
    }
}
