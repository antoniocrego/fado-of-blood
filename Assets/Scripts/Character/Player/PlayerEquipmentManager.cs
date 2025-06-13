using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    public int remainingHealthFlasks = 3; 

    public int remainingFocusFlasks = 3;

    public int remainingFocusPointsFlasks = 3;

    public bool isChugging = false;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        InitializeWeaponSlots();
    }

    protected override void Start()
    {
        LoadWeaponOnBothHands();
    }

    private void InitializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    // RIGHT WEAPON
    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            //REMOVE THE OLD WEAPON
            rightHandSlot.UnloadWeapon();

            //BRING IN THE NEW WEAPON
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void SwitchRightWeapon()
    {
        if (player.isDead) return;

        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        // ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT POTENTIAL WEAPON
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO 0
        if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }


            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                LoadRightWeapon();
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                selectedWeapon = firstWeapon;
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                LoadRightWeapon();
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
        {
            // IF THE NEXT POTENTIAL WEAPON DOES NOT EQUAL THE UNARMED WEAPON, SELECT IT
            if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                LoadRightWeapon();
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
    }
    // LEFT WEAPON
    public void LoadLeftWeapon()
    {

        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            // REMOVE THE OLD WEAPON
            leftHandSlot.UnloadWeapon();

            // BRING IN THE NEW WEAPON
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    public void SwitchLeftWeapon()
    {
        if (player.isDead) return;

        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        // ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT POTENTIAL WEAPON
        player.playerInventoryManager.leftHandWeaponIndex += 1;

        // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO 0
        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }


            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                LoadLeftWeapon();
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                selectedWeapon = firstWeapon;
                player.playerInventoryManager.currentLeftHandWeapon = selectedWeapon;
                LoadLeftWeapon();
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
        {
            // IF THE NEXT POTENTIAL WEAPON DOES NOT EQUAL THE UNARMED WEAPON, SELECT IT
            if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                LoadLeftWeapon();
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }
    }

    // DAMAGE COLLIDERS
    public void OpenDamageCollider()
    {
        if (player.isUsingRightHand)
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
        else if (player.isUsingLeftHand)
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if (player.isUsingRightHand)
        {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
        else if (player.isUsingLeftHand)
        {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
    }

    public void unHideWeapons()
    {
        if (player.playerEquipmentManager.rightHandWeaponModel != null)
        {
            player.playerEquipmentManager.rightHandWeaponModel.SetActive(true);
        }
        if (player.playerEquipmentManager.leftHandWeaponModel != null)
        {
            player.playerEquipmentManager.leftHandWeaponModel.SetActive(true);
        }
    }

    public void HideWeapons()
    {
        if (player.playerEquipmentManager.rightHandWeaponModel != null)
        {
            player.playerEquipmentManager.rightHandWeaponModel.SetActive(false);
        }
        if (player.playerEquipmentManager.leftHandWeaponModel != null)
        {
            player.playerEquipmentManager.leftHandWeaponModel.SetActive(false);
        }
    }

    public void QuickSlotItemUse(int quickSlotItemID)
    {
        QuickSlotItem item = WorldItemDatabase.Instance.GetQuickSlotItemByID(quickSlotItemID);
        item.AttemptToUseItem(player);
    }
}
