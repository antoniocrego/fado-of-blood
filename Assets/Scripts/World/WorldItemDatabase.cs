using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase Instance { get; private set; }

    public WeaponItem unarmedWeapon;

    [Header("Weapons")]
    [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

    [Header("Consumables")]

    [SerializeField] List<ConsumableItem> consumables = new List<ConsumableItem>();

    [Header("Quick Slot")]
    [SerializeField] List<QuickSlotItem> quickSlotItems = new List<QuickSlotItem>();

    [Header("Items")]
    private List<Item> items = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var consumable in consumables)
        {
            items.Add(consumable);
        }

        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }

        foreach (var item in quickSlotItems)
        {
            items.Add(item);
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(w => w.itemID == ID);
    }

    public ConsumableItem GetItemByID(int ID)
    {
        Debug.Log("The consumables.FirstOrDefault is" + consumables.FirstOrDefault(i => i.itemID == ID));
        return consumables.FirstOrDefault(i => i.itemID == ID);
    }

    public QuickSlotItem GetQuickSlotItemByID(int ID)
    {
        return quickSlotItems.FirstOrDefault(item => item.itemID == ID);
    }

    public WeaponItem GetWeaponFromSerializedData(SerializableWeapon serializedWeapon)
    {
        WeaponItem weapon = null;

        if (GetWeaponByID(serializedWeapon.weaponID))
            weapon = Instantiate(GetWeaponByID(serializedWeapon.weaponID));

        if (weapon == null)
            return Instantiate(unarmedWeapon);

        return weapon;
    }
}
