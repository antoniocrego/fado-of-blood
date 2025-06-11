using UnityEngine;

public class AIAdamastorCharacterManager : AIBossCharacterManager
{
    [HideInInspector] public AIAdamastorSoundFXManager aiAdamastorSoundFXManager;
    [HideInInspector] public AIAdamastorCombatManager aiAdamastorCombatManager;

    protected override void Start()
    {
        base.Start();
        aiAdamastorSoundFXManager = GetComponent<AIAdamastorSoundFXManager>();
    }
}