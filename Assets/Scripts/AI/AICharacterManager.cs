using UnityEngine;

public class AICharacterManager : MonoBehaviour
{
    [Header("Current State")]
    [SerializeField] private AIState currentState;

    void FixedUpdate()
    {
        ProcessStateMachine();
    }

    private void ProcessStateMachine(){
        AIState nextState = currentState?.Tick(this);

        if (nextState != null){
            currentState = nextState;
        }
    }
}