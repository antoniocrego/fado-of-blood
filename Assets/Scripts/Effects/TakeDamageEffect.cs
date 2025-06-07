using System;
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

    [Header("Direction Damage Taken From")] //this is used to know where to place the blood splatter
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager character)
    {
        if (character.isDead)
        {
            return;
        }

        CalculateDamage(character);

        PlayDamageSFX(character);
        PlayDamageVFX(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (characterCausingDamage != null)
        {
            //apply modifiers according to different characters
        }

        finalDamageDealt = damage;

        Debug.Log("Character health before taking damage: " + character.health);
        character.health -= finalDamageDealt;
        character.playerUIHudManager.SetNewHealthValue(character.health);
        Debug.Log("Character health after taking damage: " + character.health);
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

        character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
    }
}