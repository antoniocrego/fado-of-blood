using System.Linq;
using UnityEngine;

public class PlayerUIBonfireManager : PlayerUIMenu
{

    [Header("SHARD CONSUMPTION")]
    public FlaskItem mainHealthFlaskToUpgrade;    
    private string shardItemName = "Shard of Vital";
    [SerializeField] private int shardsToConsume = 1;

    [SerializeField] private int restorationIncreaseAmount = 10;

    public PlayerManager player;
    public void Awake()
    {
        player = FindAnyObjectByType<PlayerManager>();
    }

    public void ConsumeShard()
    {
        if (player == null || player.playerInventoryManager == null)
        {
            Debug.LogError("Player or PlayerInventoryManager not found in PlayerUIBonfireManager.");
            return;
        }
        Item shardItemInstance = player.playerInventoryManager.itemsInInventory.FirstOrDefault(item => item != null && item.itemName == shardItemName);

        if (shardItemInstance != null)
        {
            mainHealthFlaskToUpgrade.flaskRestoration += restorationIncreaseAmount;
            player.playerInventoryManager.RemoveItemFromInventory(shardItemInstance); 
            PlayerUIManager.instance.playerUIBonfireManager.CloseMenu();
            string successMessage = $"Consumed '{shardItemName}'. Flask potency increased.";
            PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(successMessage);
        }
        else
        {   
            string failureMessage = $"You do not have a '{shardItemName}'.";
            PlayerUIManager.instance.playerUIBonfireManager.CloseMenu();
            PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(failureMessage);
        }
    }
}