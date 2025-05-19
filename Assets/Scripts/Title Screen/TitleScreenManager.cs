using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNewGame()
    {
        WorldSaveGameManager.instance.NewGame();
        StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
    }
}
