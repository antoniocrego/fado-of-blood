using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager player;
    public int bloodDrops = 0;

    void Start()
    {
        player = GetComponent<PlayerManager>();
        if (player == null)
        {
            Debug.LogError("PlayerStatsManager requires a PlayerManager component.");
            return;
        }
    }

    public void SetBloodDrops(int amount)
    {
        bloodDrops = amount;
        PlayerUIManager.instance.playerUIHudManager.SetBloodDrops(amount);
    }

    public void AddBloodDrops(int amount)
    {
        bloodDrops += amount;
        PlayerUIManager.instance.playerUIHudManager.AddBloodDrops(amount);
        WorldSaveGameManager.instance.SaveGame();
    }
}
