using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AICharacterAttackAction : ScriptableObject
{
    [Header("Attack")]
    [SerializeField] private string attackAnimation;
    [Header("Combo Action")]
    public AICharacterAttackAction comboAction;

    [Header("Action Values")]
    public int attackWeight = 50;
    [SerializeField] AttackType attackType;
    // ATTACK CAN BE REPEATED
    public float actionRecoveryTime = 1.5f;
    public float minimumAttackAngle = -35f;
    public float maximumAttackAngle = 35f;
    public float minimumAttackDistance = 0f;
    public float maximumAttackDistance = 2f;



    public void AttemptToPerformAction(AICharacterManager aiCharacter){
        aiCharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackType, attackAnimation, true);
    }
}
