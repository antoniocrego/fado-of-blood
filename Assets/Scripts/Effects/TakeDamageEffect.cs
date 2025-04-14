using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character causing damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float damage = 0;

    [Header("Final Damage")]
    private float finalDamageDealt = 0;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager character)
    {
        if(character.isDead){
            return;
        }

        CalculateDamage(character);
    }

    private void CalculateDamage(CharacterManager character){
        if(characterCausingDamage != null){
            finalDamageDealt = damage; //change this in the future if we add modifiers
        }

        Debug.Log("Character health before taking damage: " + character.health);
        character.health -= finalDamageDealt;
        Debug.Log("Character health after taking damage: " + character.health);
    }
}