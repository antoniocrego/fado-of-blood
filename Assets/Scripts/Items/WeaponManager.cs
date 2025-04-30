using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon){
        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.damage = weapon.baseDamage;

        meleeDamageCollider.lightAttack01Modifier = weapon.lightAttack01Modifier;
    }
}
