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
        itemsInInventory.Add(item);
    }

}
