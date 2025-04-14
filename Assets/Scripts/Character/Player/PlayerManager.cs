using UnityEngine;

public class PlayerManager : CharacterManager
{
    //TODO: handles animations and stats
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;

    protected override void Awake()
    {
        base.Awake();

        playerInventoryManager = GetComponent<PlayerInventoryManager>();
    }
}