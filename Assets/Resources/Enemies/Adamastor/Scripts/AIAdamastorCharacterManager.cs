using UnityEngine;

public class AIAdamastorCharacterManager : AIBossCharacterManager
{
    [HideInInspector] public AIAdamastorCombatManager aiAdamastorCombatManager;

    protected override void Start()
    {
        base.Start();
    }
}