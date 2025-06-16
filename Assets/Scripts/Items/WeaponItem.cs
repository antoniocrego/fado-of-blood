using UnityEngine;

public class WeaponItem : Item
{
    [Header("Animations")]
    public AnimatorOverrideController weaponAnimator;

    [Header("Model Instantiation")]
    public WeaponModelType weaponModelType;
    
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Stats")]
    public float baseDamage = 10f;
    public float poiseDamage = 10f;
    public float strength = 0f;
    public float dex = 0f;
    public float magic = 0f;

    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier = 1.0f;
    public float light_Attack_02_Modifier = 1.2f;
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.6f;
    public float charge_Attack_01_Modifier = 2.0f;
    public float charge_Attack_02_Modifier = 2.2f;
    public float running_Attack_01_Modifier = 1.1f;
    public float rolling_Attack_01_Modifier = 1.1f;
    public float backstep_Attack_01_Modifier = 1.1f;
    
    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 1.0f;
    public float heavyAttackStaminaCostMultiplier = 1.3f;
    public float chargedAttackStaminaCostMultiplier = 1.5f;
    public float runningAttackStaminaCostMultiplier = 1.1f;
    public float rollingAttackStaminaCostMultiplier = 1.1f;
    public float backstepAttackStaminaCostMultiplier = 1.1f;

    [Header("Weapon Blocking Absorption")]
    public float blockingDamageAbsorption = 70;
    public float stability = 50;

    [Header("Actions")]
    public WeaponItemAction oh_RB_Action; // ONE HAND RIGHT BUMPER ACTION
    public WeaponItemAction oh_RT_Action; // ONE HAND RIGHT TRIGGER ACTION
    public WeaponItemAction oh_LB_Action;   // ONE HAND LEFT BUMPER ACTION
}
