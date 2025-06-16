using UnityEngine;
using System.Collections;

public class BloodPoolInteractable : Interactable
{
    public int bloodDrops = 0;

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        WorldSaveGameManager.instance.currentCharacterData.hasBloodPool = false;
        player.playerStatsManager.AddBloodDrops(bloodDrops);
        Destroy(gameObject);
    }
}
