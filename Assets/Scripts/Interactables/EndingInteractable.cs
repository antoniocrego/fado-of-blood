using UnityEngine;

public class EndingInteractable : Interactable
{
    [Header("Ending Interactable")]
    [SerializeField] Ending ending;

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        bool isGoodEnding = ending == Ending.Good;
        CutsceneManager.CutsceneType cutsceneType = isGoodEnding ? CutsceneManager.CutsceneType.GoodEnding : CutsceneManager.CutsceneType.BadEnding;

        WorldSaveGameManager.instance.currentCharacterData.endingID = (int)ending;
        WorldObjectManager.instance.UpdateEndingTriggers();
        WorldSaveGameManager.instance.SaveGame();

        CutsceneManager.instance.PlayCutscene(cutsceneType, () => StartCoroutine(WorldSaveGameManager.instance.LoadMainMenuScene()));
    }
}