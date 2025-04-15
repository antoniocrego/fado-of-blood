using UnityEngine;

public class WeaponItem : Item
{
    //Change attack animation according to the weapon the player is currently using
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Stats")]
    public float baseDamage = 10f;
    public float strength = 0f;
    public float dex = 0f;
    public float magic = 0f;

    //WEAPON MODIFIERS
    //LIGHT ATTACK MODIFIER
    //HEAVY ATTACK MODIFIER

    [Header("Stamina Costs")]
    public float baseStaminaCost = 10f;
}
