using UnityEngine;

public class EventTriggerForbiddenSaveArea : MonoBehaviour
{
    [Header("Forbidden Save Area Settings")]
    [SerializeField] private Vector3 safeSavePosition = new Vector3(0f, 1.2f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                WorldSaveGameManager.instance.currentCharacterData.safeSavePositionX = safeSavePosition.x;
                WorldSaveGameManager.instance.currentCharacterData.safeSavePositionY = safeSavePosition.y;
                WorldSaveGameManager.instance.currentCharacterData.safeSavePositionZ = safeSavePosition.z;
                WorldSaveGameManager.instance.currentCharacterData.isInForbiddenSaveArea = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                WorldSaveGameManager.instance.currentCharacterData.isInForbiddenSaveArea = false;
            }
        }
    }
}