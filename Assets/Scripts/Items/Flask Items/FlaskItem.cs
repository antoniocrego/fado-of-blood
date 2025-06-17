using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "Items/Consumables/Flask")]
public class FlaskItem : QuickSlotItem
{
    [Header("Flask Type")]

    public bool healthFlask = true;

    [Header("Restoration Value")]

    public int flaskRestoration = 50;

    [Header("Empty Item")]
    public GameObject emptyFlaskItem;
    public string emptyFlaskItemAnimation;

    [Header("FX")]

    [SerializeField] GameObject healingParticleFX;


    public override bool CanIUseThisItem(PlayerManager player)
    {
        if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
        {
            return false;
        }

        return true;
    }
    public override void AttemptToUseItem(PlayerManager player)
    {
        if (!CanIUseThisItem(player))
            return;

        if (healthFlask && player.playerEquipmentManager.remainingHealthFlasks <= 0)
        {
            if (player.playerCombatManager.isUsingItem)
            {
                return; 
            }
            player.playerCombatManager.isUsingItem = true;
            player.playerAnimatorManager.PlayTargetActionAnimation(emptyFlaskItemAnimation, true, false, true, true, false, true);
            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            return;
        }

        if (player.playerCombatManager.isUsingItem)
        {
            player.playerEquipmentManager.isChugging = true;

            return;
        }


        player.playerCombatManager.isUsingItem = true;

        player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandSlot.transform);

        player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true, false, true, true, false, true);

        player.playerEquipmentManager.HideWeapons();
    }

    public override void SucessfullyUseItem(PlayerManager player)
    {
        base.SucessfullyUseItem(player);

        if (healthFlask)
        {
            // Restore health
            if(player.health == player.maxHealth)
            {
                return;
            }
            if (player.health + flaskRestoration > player.maxHealth)
            {
                // Debug.Log("Player health is almost max, restoring only the remaining health.");
                // Debug.Log("Current Health: " + player.health);
                // Debug.Log("Max Health: " + player.maxHealth);
                // Debug.Log("Flask Restoration: " + flaskRestoration);
                flaskRestoration = (int)(player.maxHealth - player.health);
            }
            player.health += flaskRestoration;
            player.playerEquipmentManager.remainingHealthFlasks -= 1;
        }
        if (healthFlask && player.playerEquipmentManager.remainingHealthFlasks <= 0)
        {
            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject emptyFlask = Instantiate(emptyFlaskItem, player.playerEquipmentManager.rightHandSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
        }
        PlayHealingFX(player);
    }

    private void PlayHealingFX(PlayerManager player)
    {
        Instantiate(WorldCharacterEffectsManager.instance.healingFlaskVFX, player.transform);
        RuntimeManager.PlayOneShot("event:/Character/Healing/Healing Potion", player.transform.position);
    }


}

