using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect{
    public float staminaDamage;
    public override void ProcessEffect(CharacterManager character)
    {
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character){
        Debug.Log("Character stamina before taking damage: " + character.stamina);
        character.stamina -= staminaDamage;
        Debug.Log("Character stamina after taking damage: " + character.stamina);
    }
}