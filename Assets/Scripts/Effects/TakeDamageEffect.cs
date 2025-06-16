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

    [Header("Poise")]
    public float poiseDamage = 0;

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
        if (character.isInvulnerable) return;

        base.ProcessEffect(character);

        if (character.isDead)
        {
            return;
        }

        if (character.isInvincible)
        {
            return;
        }

        CalculateDamage(character);
        PlayDirectionalDamageVFX(character);
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
        Debug.Log("Final damage dealt: " + finalDamageDealt);

        Debug.Log("Character health before taking damage: " + character.health);
        character.health -= finalDamageDealt;
        character.playerUIHudManager.SetNewHealthValue(character.health);
        if (character is AICharacterManager)
        {
            Debug.Log("AI character health after taking damage: " + character.health);
        }
        if (character.characterHPBar != null)
            {
                character.characterHPBar.SetStat(character.health);
            }
        Debug.Log("Character health after taking damage: " + character.health);
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        character.characterSoundFXManager.PlayDamageSFX();
    }

    private void PlayDirectionalDamageVFX(CharacterManager character)
    {
        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.forwardHitAnimations);
        }
        else if (angleHitFrom >= -180 && angleHitFrom <= -145)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.forwardHitAnimations);
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.backwardHitAnimations);
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.leftHitAnimations);
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.rightHitAnimations);
        }

        character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
    }
}