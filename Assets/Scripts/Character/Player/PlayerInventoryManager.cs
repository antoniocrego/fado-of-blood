using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventoryManager : CharacterInventoryManager
{

    [Header("Weapons")]
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
    public int rightHandWeaponIndex = -1;
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
    public int leftHandWeaponIndex = -1;

    [Header("Inventory")]
    public List<Item> itemsInInventory;

    public void AddItemToInventory(Item item)
    {
        Debug.Log("Added item back to inventory: " + item.itemName);
        itemsInInventory.Add(item);
    }

    public void RemoveItemFromInventory(Item item)
    {
        itemsInInventory.Remove(item);

        for (int i = itemsInInventory.Count - 1; i > -1; i--)
        {
            if (itemsInInventory[i] == null)
            {
                itemsInInventory.RemoveAt(i);
            }
        }
    }

}
