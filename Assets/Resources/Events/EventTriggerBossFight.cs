using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{
    [SerializeField] int bossID;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AIBossCharacterManager boss = WorldAIManager.instance.GetBossByID(bossID);

        if (boss != null)
        {
            boss.WakeBoss();
        }
    }
}
