using UnityEngine;

public class ToggleBlockingController : StateMachineBehaviour
{
    PlayerManager player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("ToggleBlockingController OnStateEnter called");

        if (player == null)
            player = animator.GetComponent<PlayerManager>();

        if (player == null)
            return;

        // TO DO IN FUTURE: CHECK FOR TWO HAND STATUS

        if (player.isBlocking)
        {
            player.characterAnimatorManager.UpdateAnimatorController("coming from toggleblockingcontroller", player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);
            Debug.Log("Changed animator controller to shield");
        }
        else
        {
            player.characterAnimatorManager.UpdateAnimatorController("coming from toggleblockingcontroller", player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
            Debug.Log("Changed animator controller to sword");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            player = animator.GetComponent<PlayerManager>();

        if (player == null)
            return;

        // TO DO IN FUTURE: CHECK FOR TWO HAND STATUS

        if (player.isBlocking)
        {
            player.characterAnimatorManager.UpdateAnimatorController("coming from toggleblockingcontroller", player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);
        }
        else
        {
            player.characterAnimatorManager.UpdateAnimatorController("coming from toggleblockingcontroller", player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
        }
    }

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
