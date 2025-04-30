using UnityEngine;

public class AICharacterManager : CharacterManager
{
    [Header("Current State")]
    [SerializeField] private AIState currentState;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        ProcessStateMachine();
    }

    private void ProcessStateMachine(){
        AIState nextState = currentState?.Tick(this);

        if (nextState != null){
            currentState = nextState;
        }
    }
}