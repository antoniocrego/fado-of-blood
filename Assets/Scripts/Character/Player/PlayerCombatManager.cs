using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public WeaponItem currentWeaponBeingUsed;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
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
        player.playerCameraManager.SetLockCameraHeight();
    }
}
