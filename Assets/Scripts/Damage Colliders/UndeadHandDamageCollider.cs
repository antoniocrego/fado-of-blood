using UnityEngine;

public class UndeadHandDamageCollider : DamageCollider
{
    public AICharacterManager undeadCharacter;
    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if(charactersDamaged.Contains(damageTarget)){
            return;
        }

        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.damage = damage;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }
}