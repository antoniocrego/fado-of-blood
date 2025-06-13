using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public WeaponItem currentWeaponBeingUsed;

    public bool isUsingItem = false;

    public bool isAttacking = false;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        if (weaponAction == null || weaponPerformingAction == null) return;
        isAttacking = true;
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        if (currentWeaponBeingUsed == null)
        {
            return;
        }
        float staminaDeducted = 0;

        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttack01StaminaModifier;
                break;
            default:
                break;
        }

        player.stamina -= staminaDeducted;
        if (player.stamina <= 0)
        {
            player.stamina = 0;
        }
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);
        if (player == null)
            Debug.Log("Player is null");
        player.playerCameraManager.SetLockCameraHeight();
    }

    public void SucessfullyUseQuickSlotItem()
    {
        if(player.playerInventoryManager.currentQuickSlotItem != null)
        {
            player.playerInventoryManager.currentQuickSlotItem.SucessfullyUseItem(player);
        }
    }
}
