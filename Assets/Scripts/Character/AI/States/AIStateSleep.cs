using UnityEngine;

[CreateAssetMenu(fileName = "AIStateSleep", menuName = "AI/States/Sleep")]
public class AIStateSleep : AIState
{
    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        return base.Tick(aiCharacterManager);
    }
}