using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{

    [SerializeField] CanvasGroup[] canvasGroup;
    [SerializeField] UiStat_Bar healthBar;
    [SerializeField] UiStat_Bar staminaBar;

    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;

    [SerializeField] Image quickSlotItemQuickSlotIcon;

    [Header("Boss HUD")]
    public Transform bossHealthBarParent;
    public GameObject bossHealthBarPrefab;

    [Header("Blood Drops")]
    [SerializeField] float updateCountDelayTimer = 3f;
    private int pendingBloodDropsCount = 0;
    private Coroutine bloodDropsUpdateCoroutine;
    [SerializeField] TextMeshProUGUI bloodDropsText;
    [SerializeField] TextMeshProUGUI bloodDropsToAddText;

    public void ToggleHUD(bool status)
    {

        if (status)
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha = 1;
            }
        }
        else
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha = 0;
            }
        }
    }

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }
    public void SetNewHealthValue(float newValue)
    {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int newValue)
    {
        healthBar.SetMaxStat(newValue);
    }

    public void SetNewStaminaValue(float newValue)
    {
        staminaBar.SetStat(newValue);
    }

    public void SetMaxStaminaValue(int newValue)
    {
        staminaBar.SetMaxStat(newValue);
    }


    public void SetRightWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }
        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;
    }

    public void SetQuickSlotItemQuickSlotIcon(int itemID)
    {
        QuickSlotItem quickSlotItem = WorldItemDatabase.Instance.GetQuickSlotItemByID(itemID);

        if (quickSlotItem == null)
        {
            quickSlotItemQuickSlotIcon.enabled = false;
            quickSlotItemQuickSlotIcon.sprite = null;
            return;
        }

        if (quickSlotItem.itemIcon == null)
        {
            quickSlotItemQuickSlotIcon.enabled = false;
            quickSlotItemQuickSlotIcon.sprite = null;
            return;
        }

        quickSlotItemQuickSlotIcon.sprite = quickSlotItem.itemIcon;
        quickSlotItemQuickSlotIcon.enabled = true;
    }

    public void AddBloodDrops(int count)
    {
        pendingBloodDropsCount += count;

        if (bloodDropsUpdateCoroutine != null)
        {
            StopCoroutine(bloodDropsUpdateCoroutine);
        }

        bloodDropsUpdateCoroutine = StartCoroutine(WaitThenUpdateBloodDrops());
    }

    private IEnumerator WaitThenUpdateBloodDrops()
    {
        float timer = updateCountDelayTimer;
        int bloodDropsToAdd = pendingBloodDropsCount;
        bloodDropsToAddText.text = "+" + bloodDropsToAdd.ToString();
        bloodDropsToAddText.enabled = true;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (bloodDropsToAdd != pendingBloodDropsCount)
            {
                bloodDropsToAdd = pendingBloodDropsCount;
                bloodDropsToAddText.text = "+" + bloodDropsToAdd.ToString();
                // timer = updateCountDelayTimer; // Reset the timer if new blood drops are added.
            }

            yield return null;
        }

        bloodDropsToAddText.enabled = false;
        pendingBloodDropsCount = 0;
        bloodDropsText.text = PlayerUIManager.instance.playerManager.playerStatsManager.bloodDrops.ToString();

        yield return null;
    }

}
