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

    protected override void Awake(){
        base.Awake();

        player = GetComponent<PlayerManager>();

        InitializeWeaponSlots();
    }

    protected override void Start(){
        LoadWeaponOnBothHands();
    }

    private void InitializeWeaponSlots(){
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach(var weaponSlot in weaponSlots){
            if(weaponSlot.weaponSlot == WeaponModelSlot.RightHand){
                rightHandSlot = weaponSlot;
            }
            else if(weaponSlot.weaponSlot == WeaponModelSlot.LeftHand){
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands(){
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    // RIGHT WEAPON
    public void LoadRightWeapon(){
        if(player.playerInventoryManager.currentRightHandWeapon != null){
            //REMOVE THE OLD WEAPON
            rightHandSlot.UnloadWeapon();

            //BRING IN THE NEW WEAPON
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void SwitchRightWeapon(){
        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, true, true, true);

        WeaponItem selectedWeapon = null;

        // ADD ONE TO OUR INDEX TO SWITCH TO THE NEXT POTENTIAL WEAPON
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO 0
        if(player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2){
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for(int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++){
                if(player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if(firstWeapon == null){
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }


            if(weaponCount <= 1){
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                LoadRightWeapon();
            }
            else{
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                selectedWeapon = firstWeapon;
                player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                LoadRightWeapon();
            }

            return;
        }

        foreach(WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots){
            // IF THE NEXT POTENTIAL WEAPON DOES NOT EQUAL THE UNARMED WEAPON, SELECT IT
            if(player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID){
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                LoadRightWeapon();
            }
        }

        if(selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2){
            SwitchRightWeapon();
        }
    }
    // LEFT WEAPON
    public void LoadLeftWeapon(){

        if(player.playerInventoryManager.currentLeftHandWeapon != null){
            // REMOVE THE OLD WEAPON
            leftHandSlot.UnloadWeapon();

            // BRING IN THE NEW WEAPON
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    public void SwitchLeftWeapon(){
       
    }
}
