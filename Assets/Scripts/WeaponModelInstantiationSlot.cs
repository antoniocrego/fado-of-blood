using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public WeaponModelSlot weaponSlot; // Says where the weapon is (right hand, left hand, back, hip, etc.)
    public GameObject currentWeaponModel;

    public void UnloadWeapon(){
        if(currentWeaponModel != null){
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeapon(GameObject weaponModel){
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }
}
