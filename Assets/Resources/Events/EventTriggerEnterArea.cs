using UnityEngine;

public class EventTriggerEnterArea : MonoBehaviour
{
    [SerializeField] int areaID;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Set the current area ID in the player's save data
        CharacterSaveData playerSaveData = WorldSaveGameManager.instance.currentCharacterData;

        int currentAreaID = playerSaveData.currentAreaID;

        if (currentAreaID == areaID)
        {
            // If the player is already in this area, do nothing
            return;
        }

        playerSaveData.currentAreaID = areaID;

        // Save the updated player data
        WorldSaveGameManager.instance.SaveGame();

        PlayerUIManager.instance.playerUIPopUpManager.SendNewAreaPopUp(areaID);
    }
}