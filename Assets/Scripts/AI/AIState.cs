using UnityEngine;

public class AIState : ScriptableObject{
    public virtual AIState Tick(AICharacterManager aiCharacterManager){
        return this;
    }
}