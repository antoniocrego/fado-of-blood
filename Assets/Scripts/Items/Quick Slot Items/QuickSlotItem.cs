using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuickSlotItem : Item
{
    [Header("Item Model")]
    [SerializeField] protected GameObject itemModel;

    [Header("Animation")]
    [SerializeField] protected string useItemAnimation;

    public virtual void AttemptToUseItem(PlayerManager player)
    {
        if (!CanIUseThisItem(player))
            return;

        player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true, false,true,true);
    }

    public virtual void SucessfullyUseItem(PlayerManager player)
    {
    
    }

    public virtual bool CanIUseThisItem(PlayerManager player)
    {
        return true;
    }
}
