using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUICharacterMenuManager : PlayerUIMenu
{
    public void QuitToMenu()
    {
        PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
        PlayerUIManager.instance.CloseAllMenuWindows();

        WorldSaveGameManager.instance.SaveGame();

        StartCoroutine(WorldSaveGameManager.instance.LoadMainMenuScene());
    }
    
    public void QuitToDesktop()
    {
        WorldSaveGameManager.instance.SaveGame();

        Application.Quit();
    }
}
