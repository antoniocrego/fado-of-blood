using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Blocked Damage")]
public class TakeBlockedDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // IF A CHARACTER IS CAUSING THIS DAMAGE, THAT CHARACTER IS STORED HERE

    [Header("Damage")]
    public float damage = 0;                // BASE DAMAGE AMOUNT

    [Header("Final Damage")]
    private int finalDamageDealt = 0;         // FINAL DAMAGE TAKEN AFTER ALL CALCULATIONS HAVE BEEN MADE

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;          //  IF POISE IS BROKEN CHARACTER IS "STUNNED" AND A DAMAGE ANIMATION IS PLAYED

    //  (TO DO) BUILD UPS
    //  build up effect amounts

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX;    //  USED ON TOP OF REGULAR SFX IF THERE IS ELEMENTAL DAMAGE PRESENT (Magic/Fire/Lightning/Holy)

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;                  //  USED TO DETERMINE WHAT DAMAGE ANIMATION TO PLAY (Move backwards, to the left, to the right ect)
    public Vector3 contactPoint;                //  USED TO DETERMINE WHERE THE BLOOD FX INSTANTIATE

    public override void ProcessEffect(CharacterManager character)
    {
        if (character.isInvulnerable)
            return;

        base.ProcessEffect(character);

        Debug.Log("HIT WAS BLOCKED!");

        //  IF THE CHARACTER IS DEAD, NO ADDITIONAL DAMAGE EFFECTS SHOULD BE PROCESSED
        if (character.isDead)
            return;

        CalculateDamage(character);
        PlayDirectionalBasedBlockingAnimation(character);
        //  CHECK FOR BUILD UPS (POISON, BLEED ECT)
        PlayDamageSFX(character);
        PlayDamageVFX(character);

        //  IF CHARACTER IS A.I, CHECK FOR NEW TARGET IF CHARACTER CAUSING DAMAGE IS PRESENT
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (characterCausingDamage != null)
        {
            //  CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE (PHYSICAL/ELEMENTAL DAMAGE BUFF)
        }

        //  CHECK CHARACTER FOR FLAT DEFENSES AND SUBTRACT THEM FROM THE DAMAGE

        //  CHECK CHARACTER FOR ARMOR ABSORPTIONS, AND SUBTRACT THE PERCENTAGE FROM THE DAMAGE

        Debug.Log("ORIGINAL PHYSICAL DAMAGE: " + damage);

        damage -= damage * (character.characterStatsManager.blockingDamageAbsorption / 100);

        finalDamageDealt = Mathf.RoundToInt(damage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        Debug.Log("FINAL DAMAGE: " + finalDamageDealt);

        character.health -= finalDamageDealt;

        //  CALCULATE POISE DAMAGE TO DETERMINE IF THE CHARACTER WILL BE STUNNED
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        //  IF WE HAVE FIRE DAMAGE, PLAY FIRE PARTICLES
        //  LIGHTNING DAMAGE, LIGHTNING PARTICLES ECT

        // 1. GET VFX BASED ON BLOCKING WEAPON
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        //  IF FIRE DAMAGE IS GREATER THAN 0, PLAY BURN SFX
        //  IF LIGHTNING DAMAGE IS GREATER THAN 0, PLAY ZAP SFX

        // 1. GET SFX BASED ON BLOCKING WEAPON
    }

    private void PlayDirectionalBasedBlockingAnimation(CharacterManager character)
    {
        if (character.isDead)
            return;

        DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);
        // 2. PLAY A PROPER ANIMATION TO MATCH THE "INTENSITY" OF THE BLOW

        //  TODO: CHECK FOR TWO HAND STATUS, IF TWO HANDING USE TWO HAND VERSION OF BLOCK ANIM INSTEAD
        switch (damageIntensity)
        {
            case DamageIntensity.Ping:
                damageAnimation = "Block_Ping_01";
                break;
            case DamageIntensity.Light:
                damageAnimation = "Block_Light_01";
                break;
            case DamageIntensity.Medium:
                damageAnimation = "Block_Medium_01";
                break;
            case DamageIntensity.Heavy:
                damageAnimation = "Block_Heavy_01";
                break;
            case DamageIntensity.Colossal:
                damageAnimation = "Block_Colossal_01";
                break;
            default:
                break;
        }

        // character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
        character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
    }
}
