using UnityEngine;

public class Enums : MonoBehaviour
{

}

public enum CharacterSlot
{
    CharacterSlot01,
    CharacterSlot02,
    CharacterSlot03,
    CharacterSlot04,
    CharacterSlot05,
    NoSlot
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHandWeaponSlot,
    LeftHandShieldSlot,
}

public enum WeaponModelType
{
    Weapon,
    Shield
}

public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    HeavyAttack02,
    ChargedAttack01,
    ChargedAttack02,
    RunningAttack01,
    RollingAttack01,
    BackstepAttack01
}

public enum CharacterGroup
{
    Player, // player controlled character
    Friendly, // won't attack, can't be attacked - possibly changed to attackable by enemies
    Enemy, // will attack, can be attacked
    Neutral, // won't attack unless provoked, can be attacked
}

public enum ItemPickUpType
{
    WorldSpawn,
    CharacterDrop
}

public enum EquipmentModelType
{
    FullHelmet,     // WOULD ALWAYS HIDE FACE, HAIR ECT
    Hat,     // WOULD ALWAYS HIDE HAIR
    Hood,           // WOULD ALWAYS HIDE HAIR
    HelmetAcessorie,
    FaceCover,
    Torso,
    Back,
    RightShoulder,
    RightUpperArm,
    RightElbow,
    RightLowerArm,
    RightHand,
    LeftShoulder,
    LeftUpperArm,
    LeftElbow,
    LeftLowerArm,
    LeftHand,
    Hips,
    HipsAttachment,
    RightLeg,
    RightKnee,
    LeftLeg,
    LeftKnee
}

public enum EquipmentType
{
    RightWeapon01,
    RightWeapon02,
    RightWeapon03,
    LeftWeapon01,
    LeftWeapon02,
    LeftWeapon03,
	QuickSlotConsumable01
}

public enum DamageIntensity
{
    Ping,
    Light,
    Medium,
    Heavy,
    Colossal
}