using UnityEngine;

public class AIAdamastorCharacterManager : AIBossCharacterManager
{
    public AIAdamastorSoundFXManager aiAdamastorSoundFXManager;

    protected override void Start()
    {
        base.Start();
        aiAdamastorSoundFXManager = GetComponent<AIAdamastorSoundFXManager>();
    }
}