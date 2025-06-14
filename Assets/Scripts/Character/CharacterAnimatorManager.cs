using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{

    protected CharacterManager character;

    int vertical;
    int horizontal;

    public bool applyRootMotion = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("vertical");
        horizontal = Animator.StringToHash("horizontal");
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isMoving, bool isSpriting)
    {
        float horizontalAmount = horizontalValue;
        float verticalAmount = verticalValue;
        if (isSpriting)
        {
            if (character.isLockedOn)
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
        character.animator.SetBool("isMoving", isMoving);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false, bool canRun = true)
    {
        this.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;

        character.characterLocomotionManager.canMove = canMove;
        character.characterLocomotionManager.canRotate = canRotate; 
        
        character.characterLocomotionManager.canRun = canRun;
    }

    public virtual void PlayTargetAttackActionAnimation(WeaponItem weapon, AttackType attackType, string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false, bool canRun = true)
    {
        this.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
        UpdateAnimatorController(weapon.weaponAnimator);
        character.characterLocomotionManager.canMove = canMove;
        character.characterLocomotionManager.canRotate = canRotate;
        character.characterLocomotionManager.canRun = canRun;
        character.characterSoundFXManager.PlayAttackGruntSFX();
    }

    public void UpdateAnimatorController(AnimatorOverrideController weaponController)
    {
        character.animator.runtimeAnimatorController = weaponController;
    }
    
    public virtual void EnableCanDoCombo()
    {

    }

    public virtual void DisableCanDoCombo()
    {

    }
}