using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIEquipmentManager : PlayerUIMenu
{
    [Header("Weapon Slots")]
    [SerializeField] Image rightHandSlot01;
    [SerializeField] Image rightHandSlot02;
    [SerializeField] Image rightHandSlot03;
    [SerializeField] Image leftHandSlot01;
    [SerializeField] Image leftHandSlot02;
    [SerializeField] Image leftHandSlot03;

    [SerializeField] Image quickSlotConsumable01Icon;

    //  THIS INVENTORY POPULATES WITH RELATED ITEMS WHEN CHANGING EQUIPMENT
    [Header("Equipment Inventory")]
    public EquipmentType currentSelectedEquipmentSlot;
    [SerializeField] GameObject equipmentInventoryWindow;
    [SerializeField] GameObject equipmentInventorySlotPrefab;
    [SerializeField] Transform equipmentInventoryContentWindow;
    [SerializeField] Item currentSelectedItem;

    public override void OpenMenu()
    {
        base.OpenMenu();
        equipmentInventoryWindow.SetActive(false);

        ClearEquipmentInventory();
        RefreshWeaponSlotIcons();
        RefreshQuickSlotIcon();
    }

    public void RefreshMenu()
    {
        ClearEquipmentInventory();
        RefreshWeaponSlotIcons();
        RefreshQuickSlotIcon();
    }

    public void SelectLastSelectedEquipmentSlot()
    {
        Button lastSelectedButton = null;
        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                lastSelectedButton = rightHandSlot01.GetComponentInParent<Button>();
                break;
            case EquipmentType.RightWeapon02:
                lastSelectedButton = rightHandSlot02.GetComponentInParent<Button>();
                break;
            case EquipmentType.RightWeapon03:
                lastSelectedButton = rightHandSlot03.GetComponentInParent<Button>();
                break;
            case EquipmentType.LeftWeapon01:
                lastSelectedButton = leftHandSlot01.GetComponentInParent<Button>();
                break;
            case EquipmentType.LeftWeapon02:
                lastSelectedButton = leftHandSlot02.GetComponentInParent<Button>();
                break;
            case EquipmentType.LeftWeapon03:
                lastSelectedButton = leftHandSlot03.GetComponentInParent<Button>();
                break;
            default:
                break;
        }
        if (lastSelectedButton != null)
        {
            lastSelectedButton.Select();
            lastSelectedButton.OnSelect(null);
        }
    }

    private void RefreshQuickSlotIcon()
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();

        if (quickSlotConsumable01Icon != null) 
        {
            QuickSlotItem currentQuickSlot = player.playerInventoryManager.currentQuickSlotItem;
            if (currentQuickSlot != null && currentQuickSlot.itemIcon != null)
            {
                quickSlotConsumable01Icon.enabled = true;
                quickSlotConsumable01Icon.sprite = currentQuickSlot.itemIcon;
            }
            else
            {
                quickSlotConsumable01Icon.enabled = false;
                quickSlotConsumable01Icon.sprite = null;
            }
        }
    }

    private void RefreshWeaponSlotIcons()
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();

        //  RIGHT WEAPON 01
        WeaponItem rightHandWeapon01 = player.playerInventoryManager.weaponsInRightHandSlots[0];

        if (rightHandWeapon01.itemIcon != null)
        {
            rightHandSlot01.enabled = true;
            rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
        }
        else
        {
            rightHandSlot01.enabled = false;
        }

        //  RIGHT WEAPON 02
        WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponsInRightHandSlots[1];

        if (rightHandWeapon02.itemIcon != null)
        {
            rightHandSlot02.enabled = true;
            rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
        }
        else
        {
            rightHandSlot02.enabled = false;
        }

        //  RIGHT WEAPON 03
        WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponsInRightHandSlots[2];

        if (rightHandWeapon03.itemIcon != null)
        {
            rightHandSlot03.enabled = true;
            rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
        }
        else
        {
            rightHandSlot03.enabled = false;
        }

        //  LEFT WEAPON 01
        WeaponItem leftHandWeapon01 = player.playerInventoryManager.weaponsInLeftHandSlots[0];

        if (leftHandWeapon01.itemIcon != null)
        {
            leftHandSlot01.enabled = true;
            leftHandSlot01.sprite = leftHandWeapon01.itemIcon;
        }
        else
        {
            leftHandSlot01.enabled = false;
        }

        //  LEFT WEAPON 02
        WeaponItem leftHandWeapon02 = player.playerInventoryManager.weaponsInLeftHandSlots[1];

        if (leftHandWeapon02.itemIcon != null)
        {
            leftHandSlot02.enabled = true;
            leftHandSlot02.sprite = leftHandWeapon02.itemIcon;
        }
        else
        {
            leftHandSlot02.enabled = false;
        }

        //  LEFT RIGHT WEAPON 03
        WeaponItem leftHandWeapon03 = player.playerInventoryManager.weaponsInLeftHandSlots[2];

        if (leftHandWeapon03.itemIcon != null)
        {
            leftHandSlot03.enabled = true;
            leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
        }
        else
        {
            leftHandSlot03.enabled = false;
        }
    }

    private void ClearEquipmentInventory()
    {
        foreach (Transform item in equipmentInventoryContentWindow)
        {
            Destroy(item.gameObject);
        }
    }
    
    

    public void LoadEquipmentInventory()
    {
        equipmentInventoryWindow.SetActive(true);

        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon03:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.LeftWeapon03:
                LoadWeaponInventory();
                break;
            case EquipmentType.QuickSlotConsumable01:
                LoadQuickSlotInventory();
                break;
            default:
                break;
        }
    }

    private void LoadWeaponInventory()
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();

        List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

        // Filter based on slot type
        bool isRightHand = currentSelectedEquipmentSlot == EquipmentType.RightWeapon01 ||
                        currentSelectedEquipmentSlot == EquipmentType.RightWeapon02 ||
                        currentSelectedEquipmentSlot == EquipmentType.RightWeapon03;
        bool isLeftHand = currentSelectedEquipmentSlot == EquipmentType.LeftWeapon01 ||
                        currentSelectedEquipmentSlot == EquipmentType.LeftWeapon02 ||
                        currentSelectedEquipmentSlot == EquipmentType.LeftWeapon03;

        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;
            if (weapon != null)
            {
                if (isRightHand && weapon.weaponModelType == WeaponModelType.Weapon)
                    weaponsInInventory.Add(weapon);
                else if (isLeftHand && weapon.weaponModelType == WeaponModelType.Shield)
                    weaponsInInventory.Add(weapon);
            }
        }

        if (weaponsInInventory.Count <= 0)
        {
            RefreshMenu();
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < weaponsInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(weaponsInInventory[i]);

            //  THIS WILL SELECT THE FIRST BUTTON IN THE LIST
            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }
    
    private void EquipQuickSlotItemFromSelection(QuickSlotItem item)
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();
        if (player == null || player.playerInventoryManager == null) return;

        if (currentSelectedEquipmentSlot == EquipmentType.QuickSlotConsumable01) 
        {
            player.playerInventoryManager.currentQuickSlotItem = item;
            player.playerInventoryManager.RemoveItemFromInventory(item);
        }
        RefreshMenu(); 
        equipmentInventoryWindow.SetActive(false); 
        PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
        SelectLastSelectedEquipmentSlot(); 
    }

    public void LoadQuickSlotInventory()
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();
        if (player == null || player.playerInventoryManager == null) return;

        List<QuickSlotItem> quickSlotsInInventory = new List<QuickSlotItem>();

        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            QuickSlotItem quickSlot = player.playerInventoryManager.itemsInInventory[i] as QuickSlotItem;
            if (quickSlot != null)
                quickSlotsInInventory.Add(quickSlot);
        }

        if (quickSlotsInInventory.Count <= 0)
        {
            Debug.Log("No quick slot items in inventory to display.");
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < quickSlotsInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();

            QuickSlotItem itemToEquipOnClick = quickSlotsInInventory[i]; 
            equipmentInventorySlot.AddItem(itemToEquipOnClick);

            Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
            if (inventorySlotButton != null)
            {
                inventorySlotButton.onClick.AddListener(() => EquipQuickSlotItemFromSelection(itemToEquipOnClick));
            }

            if (!hasSelectedFirstInventorySlot && inventorySlotButton != null)
            {
                hasSelectedFirstInventorySlot = true;
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }
    }

    public void SelectEquipmentSlot(int equipmentSlot)
    {
        RefreshMenu();
        currentSelectedEquipmentSlot = (EquipmentType)equipmentSlot;
    }

    public void SelectQuickSlot(int equipmentSlot)
    {
        RefreshMenu();
        currentSelectedEquipmentSlot = (EquipmentType)equipmentSlot;
    }

    public void UnEquipSelectedItem()
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();
        Item unequippedItem;
        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponsInRightHandSlots[0] = WorldItemDatabase.Instance.unarmedWeapon;

                    if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                }
                if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                {
                    player.playerInventoryManager.currentRightHandWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                }
                break;
            case EquipmentType.RightWeapon02:
                unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponsInRightHandSlots[1] = WorldItemDatabase.Instance.unarmedWeapon;

                    if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                }
                if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                {
                    player.playerInventoryManager.currentRightHandWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                }
                break;
            case EquipmentType.RightWeapon03:
                unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponsInRightHandSlots[2] = WorldItemDatabase.Instance.unarmedWeapon;

                    if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                }
                if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                {
                    player.playerInventoryManager.currentRightHandWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                }
                break;
            case EquipmentType.LeftWeapon01:
                unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[0];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponsInLeftHandSlots[0] = WorldItemDatabase.Instance.unarmedWeapon;

                    if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                }
                if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                {
                    player.playerInventoryManager.currentLeftHandWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                }
                break;
            case EquipmentType.LeftWeapon02:
                unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[1];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponsInLeftHandSlots[1] = WorldItemDatabase.Instance.unarmedWeapon;

                    if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                }
                if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                {
                    player.playerInventoryManager.currentLeftHandWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                }
                break;
            case EquipmentType.LeftWeapon03:
                unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[2];
                if (unequippedItem != null)
                {
                    player.playerInventoryManager.weaponsInLeftHandSlots[2] = WorldItemDatabase.Instance.unarmedWeapon;

                    if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }
                }
                if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                {
                    player.playerInventoryManager.currentLeftHandWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                }
                break;
            case EquipmentType.QuickSlotConsumable01: 
                if (player.playerInventoryManager.currentQuickSlotItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.currentQuickSlotItem); 
                    player.playerInventoryManager.currentQuickSlotItem = null; 
                    Debug.Log("Unequipped Quick Slot Item from " + currentSelectedEquipmentSlot);
                }
                break;
            default:
                break;
        }
        RefreshMenu();
    }
}
