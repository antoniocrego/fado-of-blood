using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{

    [SerializeField] UiStat_Bar healthBar;
    [SerializeField] UiStat_Bar staminaBar;

    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;


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

}
