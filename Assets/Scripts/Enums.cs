using UnityEngine;

public class Enums : MonoBehaviour{

}

public enum WeaponModelSlot{
    RightHand,
    LeftHand,
}

public enum AttackType{
    LightAttack01,
}

public enum CharacterGroup{
    Player, // player controlled character
    Friendly, // won't attack, can't be attacked - possibly changed to attackable by enemies
    Enemy, // will attack, can be attacked
    Neutral, // won't attack unless provoked, can be attacked
}