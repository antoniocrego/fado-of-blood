using System;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    CharacterManager character; 

    public bool rootMotionDefault = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null)
        {
            character = animator.GetComponent<CharacterManager>();
        }

        character.isPerformingAction = false;
        character.animator.applyRootMotion = rootMotionDefault;
        character.characterAnimatorManager.applyRootMotion = rootMotionDefault;
        character.characterLocomotionManager.canMove = true;
        character.characterLocomotionManager.canRotate = true;
        character.characterAnimatorManager.DisableCanDoCombo();
        character.characterCombatManager.DisableCanDoRollingAttack();
        character.characterCombatManager.DisableCanDoBackstepAttack();
        character.isJumping = false;
        character.isInvulnerable = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    // }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
