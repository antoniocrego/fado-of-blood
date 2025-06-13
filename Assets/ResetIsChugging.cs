using UnityEngine;

public class ResetIsChugging : StateMachineBehaviour
{
    PlayerManager player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            player = animator.GetComponent<PlayerManager>();

        if (player == null)
            return;

        if (player.playerEquipmentManager.isChugging)
        {
            FlaskItem currentFlask = player.playerInventoryManager.currentQuickSlotItem as FlaskItem;

            if (currentFlask.healthFlask)
            {
                if (player.playerEquipmentManager.remainingHealthFlasks <= 0)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation(currentFlask.emptyFlaskItemAnimation, false, false, true, true, false);
                    player.playerEquipmentManager.HideWeapons();
                }
            }
            else
            {
                // if (player.playerEquipmentManager.remainingFocusPointsFlasks.Value <= 0)
                // {
                //     player.playerAnimatorManager.PlayTargetActionAnimation(currentFlask.emptyFlaskAnimation, false, false, true, true, false);
                //     player.playerNetworkManager.HideWeaponsServerRpc();
                // }
            }
        }

        //  IF WE ARE OUT OF FLASKS, INSTANTIATE THE EMPTY FLASK
        if (player.playerEquipmentManager.isChugging)
        {
            FlaskItem currentFlask = player.playerInventoryManager.currentQuickSlotItem as FlaskItem;

            if (currentFlask.healthFlask)
            {
                if (player.playerEquipmentManager.remainingHealthFlasks <= 0)
                {
                    Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                    GameObject emptyFlask = Instantiate(currentFlask.emptyFlaskItem, player.playerEquipmentManager.rightHandSlot.transform);
                    player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                }
            }
            else
            {
                // if (player.playerNetworkManager.remainingFocusPointsFlasks.Value <= 0)
                // {
                //     Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                //     GameObject emptyFlask = Instantiate(currentFlask.emptyFlaskItem, player.playerEquipmentManager.rightHandWeaponSlot.transform);
                //     player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
                // }
            }
        }

        //  RESET IS CHUGGING
        player.playerEquipmentManager.isChugging = false;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
