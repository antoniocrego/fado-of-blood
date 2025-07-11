using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
public class OffHandMeleeAction : WeaponItemAction
{
    // Q. Why call it "Off Hand Melee Action", and not "Block Action?"

    // A. In the future, if a character is wielding a main hand and off hand weapon of the same weapon class, the off hand action will not be a block.
    //    The off hand's action becomes a dual attack.

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {   
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (playerPerformingAction.isDead)
            return;

        //  CHECK FOR POWER STANCE ACTION (DUAL ATTACK)

        //  CHECK FOR CAN BLOCK
        if (!playerPerformingAction.playerCombatManager.canBlock)
            return;

        //  CHECK FOR ATTACK STATUS
        if (playerPerformingAction.isAttacking)
        {
            playerPerformingAction.isBlocking = false;
        }

        if (playerPerformingAction.isBlocking)
            return;

        playerPerformingAction.isBlocking = true;
    }
}
