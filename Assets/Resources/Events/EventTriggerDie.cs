using UnityEngine;

public class EventTriggerDie : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                player.ProcessDeath();
            }
        }
    }
}
