using UnityEngine;

public interface IWeapon
{
    string WeaponName { get; }
    GameObject WeaponModel { get; }
    void PerformAttack();
    float CalculateDamage(float playerStrength);
    void EquipWeapon(Transform weaponHolder);
}