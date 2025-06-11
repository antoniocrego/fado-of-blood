using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class UI_EquipmentInventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightedIcon;
        [SerializeField] public Item currentItem;

        public void AddItem(Item item)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
        }

        public void SelectSlot()
        {
            highlightedIcon.enabled = true;
        }

        public void DeselectSlot()
        {
            highlightedIcon.enabled = false;
        }

    public void EquipItem()
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();
        WeaponItem currentWeapon;
        switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:

                //  IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                currentWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];

                if (currentWeapon.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //  THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                player.playerInventoryManager.weaponsInRightHandSlots[0] = currentItem as WeaponItem;

                //  THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                //  RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                    player.playerInventoryManager.currentRightHandWeapon = currentItem as WeaponItem;

                //  REFRESHES EQUIPMENT WINDOW
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                break;
            case EquipmentType.RightWeapon02:
                currentWeapon = player.playerInventoryManager.weaponsInRightHandSlots[1];

                if (currentWeapon.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //  THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                player.playerInventoryManager.weaponsInRightHandSlots[1] = currentItem as WeaponItem;

                //  THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                //  RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    player.playerInventoryManager.currentRightHandWeapon = currentItem as WeaponItem;

                //  REFRESHES EQUIPMENT WINDOW
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.RightWeapon03:
                currentWeapon = player.playerInventoryManager.weaponsInRightHandSlots[2];

                if (currentWeapon.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //  THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                player.playerInventoryManager.weaponsInRightHandSlots[2] = currentItem as WeaponItem;

                //  THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                //  RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    player.playerInventoryManager.currentRightHandWeapon = currentItem as WeaponItem;

                //  REFRESHES EQUIPMENT WINDOW
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.LeftWeapon01:
                //  IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                currentWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[0];

                if (currentWeapon.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //  THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                player.playerInventoryManager.weaponsInLeftHandSlots[0] = currentItem as WeaponItem;

                //  THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                //  RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    player.playerInventoryManager.currentLeftHandWeapon = currentItem as WeaponItem;

                //  REFRESHES EQUIPMENT WINDOW
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.LeftWeapon02:
                //  IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                currentWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[1];

                if (currentWeapon.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //  THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                player.playerInventoryManager.weaponsInLeftHandSlots[1] = currentItem as WeaponItem;

                //  THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                //  RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    player.playerInventoryManager.currentLeftHandWeapon = currentItem as WeaponItem;

                //  REFRESHES EQUIPMENT WINDOW
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            case EquipmentType.LeftWeapon03:
                //  IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                currentWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[2];

                if (currentWeapon.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //  THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                player.playerInventoryManager.weaponsInLeftHandSlots[2] = currentItem as WeaponItem;

                //  THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                //  RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    player.playerInventoryManager.currentLeftHandWeapon = currentItem as WeaponItem;

                //  REFRESHES EQUIPMENT WINDOW
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            default:
                break;
        }
        PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
        }
    }

