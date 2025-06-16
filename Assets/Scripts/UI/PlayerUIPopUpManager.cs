using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("Message Pop Up")]
    [SerializeField] TextMeshProUGUI popUpMessageText;
    [SerializeField] GameObject popUpMessageGameObject;

    [Header("Item Pop Up")]
    [SerializeField] GameObject itemPopUpGameObject;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemAmount;

    [Header("YOU DIED Pop Up")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;   //  Allows us to set the alpha to fade over time

    [Header("BOSS DEFEATED Pop Up")]
    [SerializeField] GameObject bossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
    [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup;

    [Header("BONFIRE LIT Pop Up")]
    [SerializeField] GameObject bonfireLitPopUpGameObject;
    [SerializeField] TextMeshProUGUI bonfireLitPopUpText;

    [SerializeField] TextMeshProUGUI bonfireLitPopUpBackgroundText;
    [SerializeField] CanvasGroup bonfireLitPopUpCanvasGroup;

    [Header("NEW AREA Pop Up")]
    [SerializeField] GameObject newAreaPopUpGameObject;
    [SerializeField] TextMeshProUGUI newAreaPopUpText;
    [SerializeField] CanvasGroup newAreaPopUpCanvasGroup;
    [SerializeField] TextMeshProUGUI newAreaPopUpBackgroundText;

    public void CloseAllPopUpWindows()
    {
        popUpMessageGameObject.SetActive(false);

        itemPopUpGameObject.SetActive(false);

        youDiedPopUpGameObject.SetActive(false);

        bossDefeatedPopUpGameObject.SetActive(false);

        bonfireLitPopUpGameObject.SetActive(false);

        PlayerUIManager.instance.popUpWindowIsOpen = false;
    }

    public void SendPlayerMessagePopUp(string messageText)
    {
        PlayerUIManager.instance.popUpWindowIsOpen = true;
        popUpMessageText.text = messageText;
        popUpMessageGameObject.SetActive(true);
    }

    public void SendItemPopUp(Item item, int amount)
    {
        itemAmount.enabled = false;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        if (amount > 0)
        {
            itemAmount.enabled = true;
            itemAmount.text = "x" + amount.ToString();
        }

        itemPopUpGameObject.SetActive(true);
        PlayerUIManager.instance.popUpWindowIsOpen = true;
    }

    public void SendYouDiedPopUp()
    {
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
    }

    public void SendBossDefeatedPopUp(string bossDefeatedMessage = "GREAT FOE DEFEATED")
    {
        bossDefeatedPopUpText.text = bossDefeatedMessage;
        bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;
        bossDefeatedPopUpGameObject.SetActive(true);
        bossDefeatedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 8, 19));
        StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(bossDefeatedPopUpCanvasGroup, 2, 5));
    }

    public void SendBonfireLitPopUp()
    {
        bonfireLitPopUpGameObject.SetActive(true);
        bonfireLitPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(bonfireLitPopUpBackgroundText, 8, 19));
        StartCoroutine(FadeInPopUpOverTime(bonfireLitPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(bonfireLitPopUpCanvasGroup, 2, 5));
    }

    public void SendNewAreaPopUp(int areaID)
    {
        newAreaPopUpText.text = Areas.AreaNames[areaID];
        newAreaPopUpBackgroundText.text = Areas.AreaNames[areaID];
        newAreaPopUpGameObject.SetActive(true);
        newAreaPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(newAreaPopUpBackgroundText, 8, 19));
        StartCoroutine(FadeInPopUpOverTime(newAreaPopUpCanvasGroup, 2));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(newAreaPopUpCanvasGroup, 2, 1));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0f)
        {
            text.characterSpacing = 0;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if (duration > 0)
        {
            canvas.alpha = 0;
            float timer = 0;
            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {
            while (delay > 0)
            {
                delay = delay - Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 0;

        yield return null;
    }
}

