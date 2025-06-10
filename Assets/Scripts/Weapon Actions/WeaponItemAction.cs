using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;

    public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.currentWeaponBeingUsed = weaponPerformingAction;
        playerPerformingAction.playerCombatManager.currentWeaponBeingUsed = weaponPerformingAction;

        if (playerPerformingAction.currentWeaponBeingUsed != null)
        {
            playerPerformingAction.playerAnimatorManager.UpdateAnimatorController(playerPerformingAction.currentWeaponBeingUsed.weaponAnimator);
        }
    }
}