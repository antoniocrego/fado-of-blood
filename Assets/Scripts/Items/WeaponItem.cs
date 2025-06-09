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

    [Header("Attack Modifiers")]
    public float lightAttack01Modifier = 1f;

    [Header("Stamina Costs")]
    public float baseStaminaCost = 2f;
    public float lightAttack01StaminaModifier = 1f;

    [Header("Actions")]
    public WeaponItemAction oh_RB_Action; // ONE HAND RIGHT BUMPER ACTION
    public WeaponItemAction oh_RT_Action; // ONE HAND RIGHT TRIGGER ACTION
}
