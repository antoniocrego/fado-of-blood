using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour{

    CharacterManager character;

    int vertical; 
    int horizontal; 

    protected virtual void Awake() 
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("vertical");
        horizontal = Animator.StringToHash("horizontal");
    }
    public void updateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSpriting) 
    {
        float horizontalAmount = horizontalValue; 
        float verticalAmount = verticalValue;
        if(isSpriting) 
        {
            if(character.isLockedOn)
            {
                if (horizontalValue > 0) 
                {
                    horizontalAmount = 2; 
                }
                else if (horizontalValue < 0) 
                {
                    horizontalAmount = -2; 
                }
                if (verticalValue > 0) 
                {
                    verticalAmount = 2; 
                }
                else if (verticalValue < 0) 
                {
                    verticalAmount = -2; 
                }
            }
            else 
            {
                verticalAmount = 2;
            }
            
        }
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime); 
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion=true, bool canRotate = false, bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;

        character.canMove = canMove; 
        character.canRotate = canRotate; 
    }

    public virtual void PlayTargetAttackActionAnimation(AttackType attackType, string targetAnimation, bool isPerformingAction, bool applyRootMotion=true, bool canRotate = false, bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.characterCombatManager.currentAttackType = attackType;
        character.canMove = canMove; 
        character.canRotate = canRotate; 
    }
}