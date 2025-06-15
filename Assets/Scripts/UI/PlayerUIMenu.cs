using UnityEngine;
using System.Collections;

public class PlayerUIMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;

    public virtual void OpenMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = true;
        menu.SetActive(true);
    }

    public virtual void CloseMenu()
    {
        PlayerUIManager.instance.menuWindowIsOpen = false;
        menu.SetActive(false);
    }


    public virtual void CloseMenuAfterFixedFrame()
    {
        StartCoroutine(WaitThenCloseMenu());
    }

    protected virtual IEnumerator WaitThenCloseMenu()
    {
        yield return new WaitForFixedUpdate();

        PlayerUIManager.instance.menuWindowIsOpen = false;
        menu.SetActive(false);
    }
}